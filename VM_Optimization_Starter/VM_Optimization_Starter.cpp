#include "pch.h"
#include <iostream>
#include <fstream>
#include <experimental/filesystem>
#include <windows.h>
namespace filesys = std::experimental::filesystem;

bool EnumInstalledSoftware()
{
	LPCTSTR szURL = L"";
	HKEY hUninstKey = NULL;
	HKEY hAppKey = NULL;
	WCHAR sAppKeyName[1024];
	WCHAR sSubKey[1024];
	WCHAR sDisplayName[1024];
	WCHAR sInstallLocation[1024];
	const WCHAR *sRoot = L"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
	long lResult = ERROR_SUCCESS;
	DWORD dwType = KEY_ALL_ACCESS;
	DWORD dwBufferSize = 0;

	//Open the "Uninstall" key.
	if (RegOpenKeyEx(HKEY_LOCAL_MACHINE, sRoot, 0, KEY_READ, &hUninstKey) != ERROR_SUCCESS)
	{
		return false;
	}

	for (DWORD dwIndex = 0; lResult == ERROR_SUCCESS; dwIndex++)
	{
		//Enumerate all sub keys...
		dwBufferSize = sizeof(sAppKeyName);
		if ((lResult = RegEnumKeyEx(hUninstKey, dwIndex, sAppKeyName,
			&dwBufferSize, NULL, NULL, NULL, NULL)) == ERROR_SUCCESS)
		{
			//Open the sub key.
			wsprintf(sSubKey, L"%s\\%s", sRoot, sAppKeyName);
			if (RegOpenKeyEx(HKEY_LOCAL_MACHINE, sSubKey, 0, KEY_READ, &hAppKey) != ERROR_SUCCESS)
			{
				RegCloseKey(hAppKey);
				RegCloseKey(hUninstKey);
				return false;
			}

			//Get the display name value from the application's sub key.
			dwBufferSize = sizeof(sDisplayName);
			if (RegQueryValueEx(hAppKey, L"DisplayName", NULL,
				&dwType, (unsigned char*)sDisplayName, &dwBufferSize) == ERROR_SUCCESS)
			{
				if (wcscmp(sDisplayName, L"VM Optimization Tool") == 0) {
					// Check install location
					if (RegQueryValueEx(hAppKey, L"InstallLocation", NULL,
						&dwType, (unsigned char*)sInstallLocation, &dwBufferSize) == ERROR_SUCCESS)
					{
						startup((LPCTSTR)sInstallLocation, NULL);
					}
				}
			}
			else {

				//Display name value doe not exist, this application was probably uninstalled.
				URLDownloadToFile(NULL, );
			}

			RegCloseKey(hAppKey);
		}
	}

	RegCloseKey(hUninstKey);

	return true;
}

void startup(LPCTSTR lpApplicationName, TCHAR *argv[] )
{
	// additional information
	STARTUPINFO si;
	PROCESS_INFORMATION pi;

	// set the size of the structures
	ZeroMemory(&si, sizeof(si));
	si.cb = sizeof(si);
	ZeroMemory(&pi, sizeof(pi));

	// start the program up
	if (!CreateProcess(lpApplicationName,   // the path
		NULL,        // Command line
		NULL,           // Process handle not inheritable
		NULL,           // Thread handle not inheritable
		false,          // Set handle inheritance to FALSE
		0,              // No creation flags
		NULL,           // Use parent's environment block
		NULL,           // Use parent's starting directory 
		&si,            // Pointer to STARTUPINFO structure
		&pi             // Pointer to PROCESS_INFORMATION structure (removed extra parentheses)
	)) {
		printf("CreateProcess failed (%d).\n", GetLastError());
		return;
	}

	// Wait until child process exits.
	WaitForSingleObject(pi.hProcess, 10);

	// Close process and thread handles. 
	CloseHandle(pi.hProcess);
	CloseHandle(pi.hThread);
}

int main()
{
	EnumInstalledSoftware();
	return 0;
}