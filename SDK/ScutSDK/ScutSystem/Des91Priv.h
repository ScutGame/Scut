#pragma once

#if defined __cplusplus
extern "C"
{
#endif

#define ENCRYPT 0
#define DECRYPT 1

//////////////////////////////////////////////////////////////////////////

// ?¡±/¦¸?¡Ì? Type¡ã?ENCRYPT:?¡±¡Ì?,DECRYPT:¦¸?¡Ì?
void DesPriv_Run(char Out[8], char In[8], int Type);
void Des_Run(char Out[8], char In[8], int Type);
// ¡­?¡Â¡Ì¡Ì?¡®?
void DesPriv_SetKey(const char Key[8]);
void Des_SetKey(const char Key[8]);
void DesPriv_Encrypt(char*pstrIN,int length,char* pstrout,int *outlength);

//////////////////////////////////////////////////////////////////////////

#if defined __cplusplus
}
#endif