#include <Windows.h>
#include <Psapi.h>
#include <string>
#include <filesystem>

#include <iostream>

#define EnumProcessModules K32EnumProcessModules 
#define _tprintf wprintf

std::string ProcessName = "";
std::string ProcessName2 = "";

std::string utf8_encode(const std::wstring& wstr)
{
    if (wstr.empty()) return std::string();
    int size_needed = WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), NULL, 0, NULL, NULL);
    std::string strTo(size_needed, 0);
    WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), &strTo[0], size_needed, NULL, NULL);
    return strTo;
}

void GetProcessNameByID(DWORD processID, bool Change)
{
    TCHAR szProcessName[MAX_PATH] = TEXT("<unknown>");

    HANDLE hProcess = OpenProcess(PROCESS_QUERY_INFORMATION |
        PROCESS_VM_READ,
        FALSE, processID);

    std::wstring string_string;

    if (NULL != hProcess)
    {
        HMODULE hMod;
        DWORD cbNeeded;

        if (EnumProcessModules(hProcess, &hMod, sizeof(hMod),
            &cbNeeded))
        {
            GetModuleBaseName(hProcess, hMod, szProcessName,
                sizeof(szProcessName) / sizeof(TCHAR));
        }

        _tprintf(TEXT("%s  (PID: %u)\n"), szProcessName, processID);

       std::wstring string_string(szProcessName);
       ProcessName = utf8_encode(string_string);

       if (Change) {
           ProcessName2 = utf8_encode(string_string);
       }

       CloseHandle(hProcess);
    }
}

int main(int argc, char* argv[]) {

    if (argc <= 2) {
        exit(0);
    }

    ::ShowWindow(::GetConsoleWindow(), SW_HIDE);

    std::string PID = argv[1];
    std::string File_Location = argv[2];

    GetProcessNameByID(std::stoi(PID), true);

    bool IsProcessRunning = true;

    do {
        HANDLE hProcess = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, FALSE, std::stoi(PID));

        if (hProcess != NULL) {
            CloseHandle(hProcess);
            GetProcessNameByID(std::stoi(PID), false);

            if (!(ProcessName._Equal(ProcessName2))) {
                IsProcessRunning = false;
            }
        }
        else {
            IsProcessRunning = false;
        }

        Sleep(500);

    } while (IsProcessRunning);

    namespace fs = std::filesystem;
    for (fs::path p : fs::directory_iterator(File_Location)) {
        if (p.filename().compare("temp") == 0 || p.filename().compare("Songs") == 0) {
            continue;
        }

        std::string File_Location2 = File_Location + "\\temp\\old";
        fs::rename(p, File_Location2 / p.filename());
    }

    std::string File_Location2 = File_Location + "\\temp\\files";
    for (fs::path p : fs::directory_iterator(File_Location2)) {
        if (p.filename().compare("temp") == 0 || p.filename().compare("Songs") == 0) {
            continue;
        }

        fs::rename(p, File_Location / p.filename());
    }

    std::string Start = "start "" " + File_Location + "\\" + ProcessName2;

    system(Start.c_str());
    
    return 0;
}
