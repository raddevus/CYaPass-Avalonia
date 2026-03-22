
### Notes For Building


##### Requires AES_Complete Library
CYaPass does depend on `AES_Complete` library but I've just published it as a Nuget package (see [this](https://www.nuget.org/packages/AES_Complete/)) so there is no longer any challenge to the build.<br>
Now you can just 
1. clone this project (`git clone <url>`)
2. build and run `dotnet run`
3. The package will be pulled from nuget and the project will be built


1. ~~There is now a dependency on the AES_Complete library which allows SiteKeys to be Encrypted/Decrypted and sent to WebAPI for storage~~
   - ~~That project is located [here on github](https://github.com/raddevus/AES_Complete).~~
2. ~~CYaPass only uses the DLL assembly from that project (though it does produce an EXE used for driving functionality / testing~~
3. [X] 2026-03-03 - ~~Coming soon~~ there ~~will be~~ is a Nuget package for the `AES_Complete.dll`

### Backlog
Please see BACKLOG.md for list of items to be worked on and for resolved items.
### Light Mode Sample (running on macOS Sequoia)
<img width="2206" height="1198" alt="image" src="https://github.com/user-attachments/assets/b0c634f2-fad1-4f43-93ed-86480ef62c89" />

### Dark Mode Example 
-- (rough for now, to get to MVP - Minimum Viable Product)<br>
<img width="1411" height="787" alt="image" src="https://github.com/user-attachments/assets/c91b12a7-9d47-45b8-9d87-de5dcf6f458d" />
