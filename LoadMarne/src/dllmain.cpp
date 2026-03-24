// dllmain.cpp : 定义 DLL 应用程序的入口点。
#include "common.h"

//////////////////////////////////////////

// 定义原始函数指针类型
typedef BOOL(WINAPI* OriGetComputerNameA)(LPSTR lpBuffer, LPDWORD nSize);

// 指向原 GetComputerNameA 函数指针
OriGetComputerNameA pOriGetComputerNameA = nullptr;

//////////////////////////////////////////

// 默认的玩家名称
std::string playerName = "battlefield.vip";

// Hook替换函数 GetComputerNameA
BOOL WINAPI HookGetComputerNameA(LPSTR lpBuffer, LPDWORD nSize)
{
	// 一个汉字占3字节，最大5个汉字，长度为16字符
	strcpy(lpBuffer, playerName.c_str());

	return TRUE;
}

// 安装并启用Hook
void InstallHook()
{
	// 初始化MinHook
	MH_STATUS status = MH_Initialize();
	if (status != MH_OK)
		return;

	// 创建Hook
	status = MH_CreateHookApi(L"kernel32.dll", "GetComputerNameA", &HookGetComputerNameA,
		reinterpret_cast<LPVOID*>(&pOriGetComputerNameA));
	if (status != MH_OK)
		return;

	// 启用全部Hook
	status = MH_EnableHook(MH_ALL_HOOKS);
	if (status != MH_OK)
		return;
}

// 清理和卸载Hook
void CleanHook()
{
	// 禁用全部Hook
	MH_DisableHook(MH_ALL_HOOKS);
	// 卸载MinHook
	MH_Uninitialize();
}

// 内存补丁
void MemPatch(__int64 patch, BYTE value)
{
	DWORD oldProtect;
	BYTE* lpAddress;

	lpAddress = (BYTE*)patch;
	VirtualProtect(lpAddress, 1, PAGE_EXECUTE_READWRITE, &oldProtect);
	*lpAddress = value;
	VirtualProtect(lpAddress, 1, oldProtect, &oldProtect);
}

void Patch_SSL()
{
	// 先检查这个地址是否有效
	while (IsBadCodePtr((FARPROC)0x140768964))
		Sleep(1);

	// 关闭SSL证书验证
	MemPatch(0x140768964, 0x84);
}

void Patch_JS()
{
	// 先检查这个地址是否有效
	while (IsBadCodePtr((FARPROC)0x141A36EA0))
		Sleep(1);

	// 关闭Js脚本验证
	MemPatch(0x1419D55DF, 0x90);
	MemPatch(0x1419D55E3, 0x90);
	MemPatch(0x1419D55E6, 0x90);
	MemPatch(0x141A36EA0, 0xB0);
	MemPatch(0x141A36EA2, 0xC3);
}

void Patch_GPU()
{
	// 关闭显卡驱动检测
	MemPatch(0x1410365CC, 0x90);
	MemPatch(0x1410365CD, 0x90);
	MemPatch(0x1410365D2, 0x90);
	MemPatch(0x1410365D3, 0x90);
	MemPatch(0x14031CDA1, 0x84);
}

void Patch_Console()
{
	// 解锁完整命令控制台
	MemPatch(0x1403396F1, 0x90);
	MemPatch(0x1403396F2, 0x90);
	MemPatch(0x1403396F3, 0x90);
	MemPatch(0x1403396F4, 0x90);
	MemPatch(0x1403396F5, 0x90);
	MemPatch(0x1403396F6, 0x90);
}

// 核心方法
void Core()
{
	// 获取命令行字符串
	LPSTR cmdLine = GetCommandLineA();
	// 将命令行字符串转换为std::string
	std::string cmdLineString(cmdLine);

	//////////////////////////////////////////////////////////////////////////////////

	if (cmdLineString.find("-EAACBypass") != std::string::npos)
	{
		// 绕过EAAC启动限制
		HMODULE user32Ptr = GetModuleHandle(L"user32.dll");
		if (user32Ptr != nullptr)
			MemPatch((DWORD_PTR)GetProcAddress(user32Ptr, "MessageBoxTimeoutA"), 0xC3);

		HMODULE kernelbasePtr = GetModuleHandle(L"kernelbase.dll");
		if (kernelbasePtr != nullptr)
			MemPatch((DWORD_PTR)GetProcAddress(kernelbasePtr, "TerminateProcess"), 0xC3);
	}

	// 这个一般是抓包使用
	// 抓包最好不要使用JS补丁，否则可能无法上线
	if (cmdLineString.find("-PatchSSL") != std::string::npos)
	{
		Patch_SSL();
	}

	// 根据命令使用内存补丁
	if (cmdLineString.find("-UseMemPatch") != std::string::npos)
	{
		Patch_SSL();
		Patch_JS();

		Patch_GPU();
		Patch_Console();
	}

	//////////////////////////////////////////////////////////////////////////////////

	// 获取数据文件夹路径    
	PWSTR programDataPath;
	HRESULT hr = SHGetKnownFolderPath(FOLDERID_ProgramData, 0, NULL, &programDataPath);
	if (!SUCCEEDED(hr))
		return;

	// 根据命令加载Marne.dll
	if (cmdLineString.find("-UseMarne") != std::string::npos)
	{
		// 构建马恩DLL文件路径
		std::filesystem::path dllPath = std::filesystem::path(programDataPath) / "BF1MarneTools" / "Marne" / "Marne.dll";
		// 检查文件是否存在
		if (!std::filesystem::exists(dllPath))
			return;

		// 加载马恩Dll
		LoadLibraryW(dllPath.wstring().c_str());

		/////////////////////////////////////////

		// 判断是以客户端模式运行，还是服务端模式运行

		// 使用std::string::find查找子字符串
		// 如果找到了子字符串，find方法会返回它在字符串中的位置（从0开始）  
		// 如果没有找到，它会返回std::string::npos 
		if (cmdLineString.find("-mserver") != std::string::npos)
		{
			// 代表是以服务端模式运行

			ThreadSleep(20);
			// 刷新控制台状态
			std::cout.clear();

			return;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////

	// 根据命令使用自定义名称Hook
	if (cmdLineString.find("-UseCustomName") != std::string::npos)
	{
		// 下面代码只有 客户端模式 才会运行
		// 下面代码只有 客户端模式 才会运行
		// 下面代码只有 客户端模式 才会运行

		// 构建 玩家名称 文件路径
		std::filesystem::path playerNamePath = std::filesystem::path(programDataPath) / "BF1MarneTools" / "Config" / "PlayerName.txt";

		// 以二进制模式打开文件以确保正确处理所有字符
		std::ifstream fileRead(playerNamePath, std::ios::in | std::ios::binary);
		// 检查文件是否成功打开
		if (fileRead.is_open())
		{
			std::string content;
			// 读取第一行文本 
			std::getline(fileRead, content);
			// 关闭文件  
			fileRead.close();

			// 判断玩家自定义用户名是否有效
			if (!content.empty())
			{
				// 复制为玩家名称
				playerName = content;
			}
		}

		ThreadSleep(20);
		// 开始Hook
		InstallHook();
	}
}

// Dll主线程
DWORD WINAPI MainThread(LPVOID lpThreadParameter)
{
	// 核心方法
	Core();

	return TRUE;
}

// Dll加载入口
BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		DisableThreadLibraryCalls(hModule);
		if (HANDLE handle = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)MainThread, hModule, NULL, NULL))
			CloseHandle(handle);
		break;
	case DLL_THREAD_ATTACH:
		break;
	case DLL_THREAD_DETACH:
		break;
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}
