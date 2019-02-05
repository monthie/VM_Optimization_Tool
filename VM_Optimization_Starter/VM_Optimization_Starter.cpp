#include "pch.h"
#include <iostream>
#include <fstream>
#include <experimental/filesystem>
#include <windows.h>
namespace filesys = std::experimental::filesystem;

bool fexists(const char *filename) {
	std::ifstream ifile(filename);
	return (bool)ifile;
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

/*
Check if given string path is of a Directory
*/
bool checkIfDirectory(std::string filePath)
{
	try {
		// Create a Path object from given path string
		filesys::path pathObj(filePath);
		// Check if path exists and is of a directory file
		if (filesys::exists(pathObj) && filesys::is_directory(pathObj))
			return true;
	}
	catch (filesys::filesystem_error & e)
	{
		std::cerr << e.what() << std::endl;
	}
	return false;
}

int main()
{
	//path of installation
	const char* pathToTool = "C:\\Program Files\\VM Optimization Tool\\VM Optimization Tool";
	bool result = checkIfDirectory(pathToTool);
	if (result == true) {
		LPCTSTR fullPath = L"C:\\Program Files\\VM Optimization Tool\\VM Optimization Tool\\VM_Optimization_Tool.exe";
		startup((LPCTSTR)fullPath, NULL);
	}
	else {
		//start tool setup
		LPCTSTR fullPath = L"\\\\DESKTOP-AVCHJ03\\Temp\\setup.exe";
		startup((LPCTSTR)fullPath, NULL);
	}
	return 0;
}