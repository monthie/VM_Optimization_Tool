// VM_Optimization_Starter.cpp : Diese Datei enthält die Funktion "main". Hier beginnt und endet die Ausführung des Programms.
//

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
	const char* pathToTool = "C:\\Users\\Du\\source\\repos\\VM_Optimization_Tool\\VM_Optimization_Tool\\bin\\Debug";
	bool result = checkIfDirectory(pathToTool);
	if (result == true) {
		LPCTSTR fullPath = L"C:\\Users\\Du\\source\\repos\\VM_Optimization_Tool\\VM_Optimization_Tool\\bin\\Debug\\VM_Optimization_Tool.exe";
		startup((LPCTSTR)fullPath, NULL);
	}
	return 0;
}



// Programm ausführen: STRG+F5 oder "Debuggen" > Menü "Ohne Debuggen starten"
// Programm debuggen: F5 oder "Debuggen" > Menü "Debuggen starten"

// Tipps für den Einstieg: 
//   1. Verwenden Sie das Projektmappen-Explorer-Fenster zum Hinzufügen/Verwalten von Dateien.
//   2. Verwenden Sie das Team Explorer-Fenster zum Herstellen einer Verbindung mit der Quellcodeverwaltung.
//   3. Verwenden Sie das Ausgabefenster, um die Buildausgabe und andere Nachrichten anzuzeigen.
//   4. Verwenden Sie das Fenster "Fehlerliste", um Fehler anzuzeigen.
//   5. Wechseln Sie zu "Projekt" > "Neues Element hinzufügen", um neue Codedateien zu erstellen, bzw. zu "Projekt" > "Vorhandenes Element hinzufügen", um dem Projekt vorhandene Codedateien hinzuzufügen.
//   6. Um dieses Projekt später erneut zu öffnen, wechseln Sie zu "Datei" > "Öffnen" > "Projekt", und wählen Sie die SLN-Datei aus.
