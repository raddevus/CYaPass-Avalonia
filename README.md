<img width="1411" height="787" alt="image" src="https://github.com/user-attachments/assets/c91b12a7-9d47-45b8-9d87-de5dcf6f458d" />

### Notes For Building
##### Requires AES_Complete Library
1. There is now a dependency on the AES_Complete library which allows SiteKeys to be Encrypted/Decrypted and sent to WebAPI for storage
   - That project is located [here on github](https://github.com/raddevus/AES_Complete).
2. CYaPass only uses the DLL assembly from that project (though it does produce an EXE used for driving functionality / testing

## Backlog Items
-  [ - ] Remove hard-coded values, in ImportSiteKey method & read them from the user
     -  [ - ] demoKeys2022 - read maintoken from user
     -  [ - ] base url to LibreStore api (add config stored with the app)
-  [ - ] Calculate HMAC & compare to transfered value before attempting to decrypt data
-  [ X ] Change MaxLength control from TextBox to NumericUpDown - 2026-02-07
-  [ X ] Add NumericUpDown to Multi-hash - 2026-02-07
-  [ - ] Alter SiteKey dialog to take all values from user
   - [ - ] maxLength - int value > 0 (0 means no maxLength)
   - [ - ] special chars - bool
   - [ - ] uppercase - bool
-  [ - ] Save SiteKeys as JSON in local file (make sure JSON format matches current CYaPass versions exactly)
-  [ - ] Fill SiteKeys list box from values read from local file (if exists)
-  [ - ] Implement Export SiteKeys button
-  [ X ] Implement Add Uppercase - 2026-02-13
-  [ X ] Implement Add Special Char - 2026-02-14
-  [ X ] Implement Max Length - 2026-02-13
-  [ - ] Clean up UI - various padding, margins etc.
-  [ - ] Implement Edit (SiteKey) button
-  [ - ] Implement Delete (SiteKey) button
-  [ X ] App Config - 2026-02-18
   - [ X ] `multiHashIsOn`   `multiHashCount` Save Multi-hash value to config when user changes it - keep track of value for user 2026-02-18
   - [ X ] `lastSelectedKey` Insure that the last selected SiteKey is always saved so that when user starts app again it is selected - 2026-02-17
   - [ X ] `transferUrl` Insure the transfer URL that is used is saved for next time 2026-02-18
-  [ - ] TextCopy - Install on various Linux systems (via VirtualBox) test how it behaves (Fedora, Ubuntu, Debian, Manjaro, OMarchy)
-  [ X ] Need to insure that if MultiHash is turned on that the value is used when the grid is drawn (segments added)
   -  [ X ] 2026-02-13 This issue is resovled by moving properties to PwdGrid class
- [ - ] Clean up all build warnings
- [ X ] Add cyapass icon - 2026-02-18
- [ - ] 

#### No Longer Requires TextCopy DLL - Now Uses Avalonia for Copy To Clipboard 
- Tested across multiple OSes (OpenMandriva, Fedora, Win11, macOS(Sequoia) and it works great
##### ~~Requires (custom) TextCopy Library~~
~~1. There is now a dependency upon the (custom) TextCopy library which I've forked [here on github](https://github.com/raddevus/TextCopy).~~
~~2. The forked TextCopy project is updated to .NET 9.x but you can really only build the TextCopy.dll (TextCopy project) located under `/src/TextCopy`~~
   ~~- I don't care about the blazor project or any of that other stuff - I had to fix it to work on other Linux distros
3. After you build the TextCopy project you need to A) create a directory named `/external` in the CYaPass_Avalonia project (in the same directory where CYaPass_Avalonia.csproj is found
4. That project simply allows CYaPass to copy the password to the clipboard.~~
