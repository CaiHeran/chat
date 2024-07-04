
#include <string>

#define OPENSSL_NO_DEPRECATED
#include <openssl/pem.h>
#include <openssl/rsa.h>
#include <openssl/x509.h>

auto make_cert(std::string CN)
{
	X509 *x509 = X509_new();
	ASN1_INTEGER_set(X509_get_serialNumber(x509), 1);
	X509_gmtime_adj(X509_getm_notBefore(x509), 0);
	X509_gmtime_adj(X509_getm_notAfter(x509), 3600L);

	EVP_PKEY *pkey = EVP_RSA_gen(2048);
	X509_set_pubkey(x509, pkey);

	X509_NAME *name;
	name = X509_get_subject_name(x509);

	X509_NAME_add_entry_by_txt(name,"CN", MBSTRING_ASC,
		(unsigned char*)CN.c_str(), CN.length(), -1, 0);
	X509_set_issuer_name(x509, name);
	X509_sign(x509, pkey, EVP_sha256());

	// 导出私钥
	BIO *bio = BIO_new(BIO_s_mem());
	PEM_write_bio_PrivateKey(bio, pkey, nullptr, nullptr, 0, nullptr, nullptr);
	EVP_PKEY_free(pkey);
	int len = BIO_ctrl_pending(bio);
	std::string prikey;
	prikey.resize(len);
	BIO_read(bio, prikey.data(), len);
	BIO_free(bio);
	// 导出x509证书
	std::string cert;
	bio = BIO_new(BIO_s_mem());
	PEM_write_bio_X509(bio, x509);
	X509_free(x509);
	len = BIO_ctrl_pending(bio);
	cert.resize(len);
	BIO_read(bio, cert.data(), len);
	BIO_free(bio);

	return std::make_tuple(cert, prikey);
}