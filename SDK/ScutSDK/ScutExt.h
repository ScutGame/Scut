#pragma once

#include <string>

class ScutExt
{
private:
	ScutExt()
	{
		m_pSceneChangeCallback = NULL;
		m_strPauseHandler = "";
		m_strResumeHandler = "";
		m_strBackHandler = "";
		m_strErrorHandler = "";
		m_strSocketPushHandler = "";
		m_strSocketErrorHandler = "";
	}

    
public:
	static void Init(const std::string& resRootDir);
    static const std::string& getResRootDir();
	static ScutExt* getInstance();
	
	/** Pauses the running scene for lua
	 */
	void RegisterPauseHandler(const char* pszFuncName);

	/** Resumes the paused scene for lua
	 */
	void RegisterResumeHandler(const char* pszFuncName);

	/** register back for lua
	 */

	typedef void(*SCENE_CHANGE_CALLBACK)(void* pSender, int nType);
	void registerSceneChangeCallback(SCENE_CHANGE_CALLBACK fun);

	void RegisterBackHandler(const char* pszFuncName);

	/** register error log for lua
	 */
	void RegisterErrorHandler(const char* pszFuncName);

	/** register Push log for lua
	 */
	void RegisterSocketPushHandler(const char* pszFuncName);
	const char* GetSocketPushHandler();

	/** register Socket Error log for lua
	 */
	void RegisterSocketErrorHandler(const char* pszFuncName);
	const char* GetSocketErrorHandler();

	/** unregister error log for lua
	 */
	void UnregisterErrorHandler();
	/** get registered error log handler
	*/
	std::string& GetErrorHandler();

	/** excute back for lua
	 */
	bool ExcuteBackHandler();

	void EndDirector();
	void PauseDirector();
	void ResumeDirector();

	// CCLuaStack
	bool pushfunc(const std::string & strFunc);
	void executeLogEvent( std::string& func, std::string& errlog );

protected:
	SCENE_CHANGE_CALLBACK m_pSceneChangeCallback;

	std::string m_strPauseHandler;
	std::string m_strResumeHandler;
	std::string m_strBackHandler;
	std::string m_strErrorHandler;
	std::string m_strSocketPushHandler;
	std::string m_strSocketErrorHandler;

	static ScutExt* sInstance;
    static std::string sResRootDir;
};