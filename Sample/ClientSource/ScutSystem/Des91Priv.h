#if defined __cplusplus
extern "C"
{
#endif

#define ENCRYPT 0
#define DECRYPT 1

//////////////////////////////////////////////////////////////////////////

// ?¡±/¦¸?¡Ì? Type¡ã?ENCRYPT:?¡±¡Ì?,DECRYPT:¦¸?¡Ì?
extern void DesPriv_Run(char Out[8], char In[8], int Type);
extern void Des_Run(char Out[8], char In[8], int Type);
// ¡­?¡Â¡Ì¡Ì?¡®?
extern void DesPriv_SetKey(const char Key[8]);
	
extern void Des_SetKey(const char Key[8]);
	
	
extern void DesPriv_Encrypt(char*pstrIN,int length,char* pstrout,int *outlength);

//////////////////////////////////////////////////////////////////////////

#if defined __cplusplus
}
#endif