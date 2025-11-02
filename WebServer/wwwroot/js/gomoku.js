// 五子棋前端交互脚本

let connection = null;
let gameId = null;
let currentUserId = null;
let gameState = null;
let firstStateArrived = false;

const BOARD_PADDING = 20;
const CELL_SIZE = 35;
const STONE_RADIUS = CELL_SIZE / 2 - 3;

/**
 * 初始化五子棋游戏
 */
function initGomoku(gId, uId) {
	gameId = gId;
	currentUserId = uId;

	console.log("Gomoku init:", { gameId, currentUserId });

	// 建立 SignalR连接
	connection = new signalR.HubConnectionBuilder()
		.withUrl("/gomokuhub")
		.withAutomaticReconnect()
		.build();

	//监听服务端事件
	connection.on("GameStateUpdated", (state) => {
		firstStateArrived = true;
		handleGameStateUpdated(state);
	});
	connection.on("GameOver", handleGameOver);
	connection.on("Error", handleError);

	connection.onreconnecting(err => {
		console.warn("gomokuhub reconnecting", err);
		showMessage('正在重新连接实时服务...', 'warning');
	});
	connection.onreconnected(id => {
		console.warn("gomokuhub reconnected", id);
		showMessage('已重新连接实时服务，正在恢复会话...', 'info');
		// 重新加入分组，确保能收到组广播
		if (gameId) {
			connection.invoke('JoinGame', gameId).catch(err => console.error('Re-JoinGame failed:', err));
			// 主动请求一次状态以保障 UI 更新
			connection.invoke('RequestState', gameId).catch(err => console.error('RequestState failed after reconnect:', err));
		}
	});
	connection.onclose(err => {
		console.warn("gomokuhub closed", err);
		showMessage('与实时服务的连接已关闭', 'danger');
	});

	// 启动连接
	connection.start()
		.then(() => {
			console.log("✓ SignalR连接成功");
			// 加入对局，服务端会在加入后自动推送状态
			connection.invoke("JoinGame", gameId)
				.catch(err => console.error("加入对局失败：", err));
		})
		.catch(err => {
			console.error("✗ SignalR连接失败：", err);
			showMessage("连接失败，请刷新页面重试", "danger");
			//立即走一次 HTTP 回退
			fetchStateHttp();
		});

	// 0.8 秒后若还没收到状态，走一次 HTTP 回退
	setTimeout(() => { if (!firstStateArrived) fetchStateHttp(); }, 800);

	// 绑定UI事件
	bindUIEvents();
}

/** 获取状态（HTTP 回退） */
async function fetchStateHttp() {
	try {
		const res = await fetch(`/api/gomoku/${gameId}`);
		if (!res.ok) throw new Error("HTTP 获取状态失败");
		const state = await res.json();
		console.log("HTTP state:", state);
		handleGameStateUpdated(state);
	} catch (e) {
		console.error("HTTP 获取状态异常", e);
	}
}

/**
 *绑定 UI事件
 */
function bindUIEvents() {
	const board = document.getElementById("gomoku-board");
	const startBtn = document.getElementById("start-game-btn");
	const resignBtn = document.getElementById("resign-btn");
	const requestStateBtn = document.getElementById("request-state-btn");

	// 棋盘点击事件
	board.addEventListener("click", handleBoardClick);

	// 开始按钮
	if (startBtn) {
		startBtn.addEventListener("click", () => {
			connection.invoke("StartGame", gameId)
				.catch(err => console.error("开始对局失败：", err));
		});
	}

	// 认输按钮
	if (resignBtn) {
		resignBtn.addEventListener("click", () => {
			if (confirm("确定要认输吗？")) {
				connection.invoke("Resign", gameId)
					.catch(err => console.error("认输失败：", err));
			}
		});
	}

	// 刷新状态按钮（同时支持 HTTP 回退）
	if (requestStateBtn) {
		requestStateBtn.addEventListener("click", async () => {
			try {
				await connection.invoke("RequestState", gameId);
			} catch (err) {
				console.error("请求状态失败：", err);
			} finally {
				// 总是补一次 HTTP 回退，确保 UI 刷新
				fetchStateHttp();
			}
		});
	}
}

/**
 * 棋盘点击处理
 */
function handleBoardClick(event) {
	if (!gameState || gameState.status !== 1) { //1 = Playing
		showMessage("对局未开始或已结束", "warning");
		return;
	}

	if (gameState.currentTurnPlayerId !== currentUserId) {
		showMessage("现在不是你的回合", "warning");
		return;
	}

	const rect = event.target.getBoundingClientRect();
	const x = event.clientX - rect.left;
	const y = event.clientY - rect.top;

	// 计算棋盘坐标
	const col = Math.round((x - BOARD_PADDING) / CELL_SIZE);
	const row = Math.round((y - BOARD_PADDING) / CELL_SIZE);

	// 边界检查
	if (col < 0 || col >= gameState.boardSize || row < 0 || row >= gameState.boardSize) {
		showMessage("落子超出棋盘范围", "warning");
		return;
	}

	// 检查该位置是否已有棋子
	if (gameState.moves.some(m => m.x === col && m.y === row)) {
		showMessage("该位置已有棋子", "warning");
		return;
	}

	//发送落子请求
	connection.invoke("MakeMove", gameId, col, row)
		.catch(err => console.error("落子失败：", err));
}

/**
 * 处理游戏状态更新
 */
function handleGameStateUpdated(state) {
	gameState = state;
	console.log("游戏状态更新:", state);

	updateGameUI(state);
	redrawBoard(state);
}

/**
 * 处理游戏结束
 */
function handleGameOver(result) {
	// Support both camelCase (SignalR JSON) and PascalCase payloads
	const winnerId = result?.winnerId ?? result?.WinnerId ?? null;
	const blackId = result?.blackPlayerId ?? result?.BlackPlayerId ?? (gameState ? gameState.blackPlayerId : null);

	let winnerName = '白方';
	if (winnerId && blackId && winnerId === blackId) {
		winnerName = '黑方';
	}

	showMessage(`游戏结束！${winnerName}获胜！`, 'success');
	const resignBtn = document.getElementById('resign-btn');
	const startBtn = document.getElementById('start-game-btn');
	if (resignBtn) resignBtn.disabled = true;
	if (startBtn) startBtn.disabled = true;
}

/**
 * 处理错误信息
 */
function handleError(message) {
	console.error("服务端错误：", message);
	showMessage(message, "danger");
}

/**
 * 更新游戏 UI
 */
function updateGameUI(state) {
	// 棋盘大小
	document.getElementById("board-size").textContent = `${state.boardSize}×${state.boardSize}`;

	// 玩家信息
	const blackName = state.blackPlayerId ? `${state.blackPlayerId.substring(0, 8)}...` : "等待中";
	const whiteName = state.whitePlayerId ? `${state.whitePlayerId.substring(0, 8)}...` : "等待中";
	document.getElementById("black-player").textContent = blackName;
	document.getElementById("white-player").textContent = whiteName;

	// 当前回合
	const statusMap = { 0: "等待中", 1: "进行中", 2: "已结束" };
	let statusText = statusMap[state.status] || "未知";

	if (state.status === 1 && state.currentTurnPlayerId) {
		const turnColor = state.currentTurnPlayerId === state.blackPlayerId ? "黑方" : "白方";
		statusText = `${statusText} -轮到${turnColor}`;
	}
	document.getElementById("current-turn").textContent = statusText;

	// 落子数
	document.getElementById("move-count").textContent = state.moves.length;

	// 更新按钮状态
	const startBtn = document.getElementById("start-game-btn");
	const resignBtn = document.getElementById("resign-btn");

	if (state.status === 0 && state.blackPlayerId === currentUserId && state.whitePlayerId) {
		startBtn.disabled = false;
	} else {
		startBtn.disabled = true;
	}

	if (state.status === 1 && (state.blackPlayerId === currentUserId || state.whitePlayerId === currentUserId)) {
		resignBtn.disabled = false;
	} else {
		resignBtn.disabled = true;
	}

	// 更新游戏状态显示
	const statusDiv = document.getElementById("game-status");
	if (state.status === 0) {
		if (state.whitePlayerId) {
			statusDiv.innerHTML = '<strong style="color: #28a745;">✓ 两位玩家已准备，等待黑方开始对局</strong>';
			statusDiv.className = "alert alert-success mt-2";
		} else {
			statusDiv.innerHTML = '<strong style="color: #ffc107;">⏳ 等待白方加入...</strong>';
			statusDiv.className = "alert alert-warning mt-2";
		}
	} else if (state.status === 1) {
		const turnColor = state.currentTurnPlayerId === state.blackPlayerId ? "黑方" : "白方";
		const isMyTurn = state.currentTurnPlayerId === currentUserId;
		statusDiv.innerHTML = `<strong style="color: ${isMyTurn ? '#28a745' : '#007bff'};">${isMyTurn ? '●现在轮到你' : '⊙ ' + turnColor + '的回合'}</strong>`;
		statusDiv.className = isMyTurn ? "alert alert-success mt-2" : "alert alert-info mt-2";
	} else if (state.status === 2) {
		statusDiv.innerHTML = '<strong style="color: #dc3545;">✗ 对局已结束</strong>';
		statusDiv.className = "alert alert-danger mt-2";
	}
}

/**
 * 重绘棋盘
 */
function redrawBoard(state) {
	const canvas = document.getElementById("gomoku-board");
	const ctx = canvas.getContext("2d");
	const boardSize = state.boardSize;

	// 清空画布
	ctx.fillStyle = "#f0e68c";
	ctx.fillRect(0, 0, canvas.width, canvas.height);

	// 绘制棋盘网格
	ctx.strokeStyle = "#000";
	ctx.lineWidth = 1;
	for (let i = 0; i < boardSize; i++) {
		const x = BOARD_PADDING + i * CELL_SIZE;
		const y = BOARD_PADDING + i * CELL_SIZE;

		// 水平线
		ctx.beginPath();
		ctx.moveTo(BOARD_PADDING, y);
		ctx.lineTo(BOARD_PADDING + (boardSize - 1) * CELL_SIZE, y);
		ctx.stroke();

		// 竖直线
		ctx.beginPath();
		ctx.moveTo(x, BOARD_PADDING);
		ctx.lineTo(x, BOARD_PADDING + (boardSize - 1) * CELL_SIZE);
		ctx.stroke();
	}

	// 绘制棋子
	state.moves.forEach(move => {
		const x = BOARD_PADDING + move.x * CELL_SIZE;
		const y = BOARD_PADDING + move.y * CELL_SIZE;

		ctx.beginPath();
		ctx.arc(x, y, STONE_RADIUS, 0, 2 * Math.PI);
		ctx.fillStyle = move.playerId === state.blackPlayerId ? "#000" : "#fff";
		ctx.fill();
		ctx.strokeStyle = "#000";
		ctx.lineWidth = 1;
		ctx.stroke();
	});
}

/**
 * 显示消息
 */
function showMessage(message, type = "info") {
	const messageBox = document.getElementById("message-box");
	const timestamp = new Date().toLocaleTimeString();

	const msgDiv = document.createElement("div");
	msgDiv.className = `alert alert-${type} mb-2`;
	msgDiv.style.fontSize = "12px";
	msgDiv.innerHTML = `<small>[${timestamp}] ${message}</small>`;

	messageBox.insertBefore(msgDiv, messageBox.firstChild);

	// 最多保留20条消息
	while (messageBox.children.length > 20) {
		messageBox.removeChild(messageBox.lastChild);
	}
}
