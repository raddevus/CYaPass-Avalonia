## Backlog Items
- [ X ] Fix issue with import URL related to trailing slash - figure out how that has to be formatted 2026-02-27 <br>
      - This issue was resolved int the AES_Complete project by using a call to URI() to build the URL properly.
- [ X ] Handle all errors on Import & provide message to user when errors occur. 2026-03-02
- [ X ] Make sure the SiteKeys are saved to file every time they change (instead of when program closes). 2026-02-28
- [ X ] Implement Export SiteKeys button 2026-03-02
- [ - ] After Delete of SiteKey - set the selected item to one that is next to the one that was deleted
- [ - ] Implement fast find to SiteKey - legacy app allows user to press letter that matches first letter of sitkey & iterates over them.
-  [ - ] Clean up UI - various padding, margins etc.
-  [ - ] Implement Edit (SiteKey) button
- [ - ] Clean up dailog boxes controls & set sizes to they display properly
- [ - ] Provide alert dialog when user attempts to delete a sitekey
- [ - ] Provide alert dialog when user attemps to delete **all** sitekeys
- [ - ] Clean up all build warnings
- [ X ] **Fix the issue that occurs when User Removes All SiteKeys and then adds one new SiteKey** -- the listbox doesn't display it.<br>
But, if user closes the app the sitekey is saved on close and then starts again the sitekey is displayed in listbox. 2026-02-26
-  [ X ] Remove hard-coded values, in ImportSiteKey method & read them from the user 2026-02-24
     -  [ X ] demoKeys2022 - read maintoken from user 2026-02-24
     -  [ X ] base url to LibreStore api (add config stored with the app) 2026-02-24
-  [ X ] Calculate HMAC & compare to transfered value before attempting to decrypt data 2026-03-02
-  [ X ] Change MaxLength control from TextBox to NumericUpDown - 2026-02-07
-  [ X ] Add NumericUpDown to Multi-hash - 2026-02-07
-  [ X ] Alter SiteKey dialog to take all values from user - 2026-02-22
   - [ X ] maxLength - int value > 0 (0 means no maxLength) 2026-02-22
   - [ X ] special chars - bool 2026-02-22
   - [ X ] uppercase - bool 2026-02-22
-  [ X ] Save SiteKeys as JSON in local file (make sure JSON format matches current CYaPass versions exactly) 2026-02-25
-  [ X ] Fill SiteKeys list box from values read from local file (if exists) 2026-02-25
-  [ X ] Implement Add Uppercase - 2026-02-13
-  [ X ] Implement Add Special Char - 2026-02-14
-  [ X ] Implement Max Length - 2026-02-13
-  [ X ] Implement Delete (SiteKey) button 2022-02-20
-  [ X ] App Config - 2026-02-18
   - [ X ] `multiHashIsOn`   `multiHashCount` Save Multi-hash value to config when user changes it - keep track of value for user 2026-02-18
   - [ X ] `lastSelectedKey` Insure that the last selected SiteKey is always saved so that when user starts app again it is selected - 2026-02-17
   - [ X ] `transferUrl` Insure the transfer URL that is used is saved for next time 2026-02-18
-  [ X ] TextCopy - Install on various Linux systems (via VirtualBox) test how it behaves (Fedora, Ubuntu, Debian, Manjaro, OMarchy)
-   -  No longer uses TextCopy but I did test Avalonia-code copy to clipboard on multiple environments 
-  [ X ] Need to insure that if MultiHash is turned on that the value is used when the grid is drawn (segments added)
   -  [ X ] 2026-02-13 This issue is resovled by moving properties to PwdGrid class

- [ X ] Add cyapass icon - 2026-02-18


#### No Longer Requires TextCopy DLL - Now Uses Avalonia for Copy To Clipboard 
- Tested across multiple OSes (OpenMandriva, Fedora, Win11, macOS(Sequoia) and it works great
##### ~~Requires (custom) TextCopy Library~~
~~1. There is now a dependency upon the (custom) TextCopy library which I've forked [here on github](https://github.com/raddevus/TextCopy).~~
~~2. The forked TextCopy project is updated to .NET 9.x but you can really only build the TextCopy.dll (TextCopy project) located under `/src/TextCopy`~~
   ~~- I don't care about the blazor project or any of that other stuff - I had to fix it to work on other Linux distros
3. After you build the TextCopy project you need to A) create a directory named `/external` in the CYaPass_Avalonia project (in the same directory where CYaPass_Avalonia.csproj is found
4. That project simply allows CYaPass to copy the password to the clipboard.~~
