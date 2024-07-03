#pragma once
#ifndef __BASICS_HPP__
#define __BASICS_HPP__

#include "headers.hpp"

//枚举数据类型：颜色
enum Color : std::int8_t {Wrong=-1, None, Red, Green, Blue, Yellow};

struct Operation     //出牌记录
{
	Color color;     	//玩家出牌颜色，None且wild==0表示pass
	int basic, plus, wild; //基本牌数，+1牌数，万能牌数
	// int plus_n;		// +1牌中选为2的张数
	constexpr int tot() const
	  { return basic + 2*plus + wild; }

	Operation() = default;
	Operation(Color _color, int _basic, int _plus, int _wild)
	  : color(_color),
	  	basic(_basic),
		plus(_plus),
		wild(_wild)
		{}
	explicit Operation(json info)
	  : color((Color)info["color"].get<int>()),
		basic(info["basic"].get<int>()),
		plus(info["plus"].get<int>()),
		wild(info["wild"].get<int>())
		{}
	explicit operator json() const
	{
		return json {
			{"color", color},
			{"basic", basic},
			{"plus", plus},
			{"wild", wild}
		};
	}
};

struct Cards
{
	int R,G,B,Y;       //各色基本牌
	int RP,GP,BP,YP;   //各色+1牌
	int Wild;          //万能牌

	constexpr Cards& operator -= (const Cards& m) noexcept
	{
		R -= m.R; G -= m.G; B -= m.B; Y -= m.Y; 
		RP -= m.RP; GP -= m.GP; BP -= m.BP; YP -= m.YP; 
		Wild -= m.Wild;
		return *this;
	}
	constexpr Cards operator - (const Cards& m) const noexcept
	{
		Cards t = *this;
		return t -= m;
	}

	constexpr int count() const noexcept
	{
		return R+G+B+Y+RP+GP+BP+YP+Wild;
	}

	Cards() = default;
	Cards(int R, int G, int B, int Y, int RP, int GP, int BP, int YP, int Wild)
	  : R(R), G(G), B(B), Y(Y), RP(RP), GP(GP), BP(BP), YP(YP), Wild(Wild)
		{}
	explicit Cards(json arr)
	{
		auto it = arr.begin();
		for (int i=0; i<9; i++)
			*(reinterpret_cast<int*>(this) + i) = *it++;
	}

	Cards(const Operation op) {
		R = G = B = Y = RP = GP = BP = YP = 0;
		Wild = op.wild;
		switch (op.color)
		{
		case Red:
			R += op.basic;
			RP += op.plus;
			break;
		case Green:
			G += op.basic;
			GP += op.plus;
		case Blue:
			B += op.basic;
			BP += op.plus;
		case Yellow:
			Y += op.basic;
			YP += op.plus;
		}
		return;
	}

	explicit operator json() const {
    	return json::array_t((int*)this, (int*)this + 9);
	}
};

#endif