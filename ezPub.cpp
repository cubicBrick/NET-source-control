#include <cstdlib>
int main(void){
    std::system("dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true");
}