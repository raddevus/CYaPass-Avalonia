### Notes For Building
##### Requires (custom) TextCopy Library
1. There is now a dependency upon the (custom) TextCopy library which I've forked [here on github](https://github.com/raddevus/TextCopy).
2. The forked TextCopy project is updated to .NET 9.x but you can really only build the TextCopy.dll (TextCopy project) located under `/src/TextCopy`
   - I don't care about the blazor project or any of that other stuff - I had to fix it to work on other Linux distros
3. After you build the TextCopy project you need to A) create a directory named `/external` in the CYaPass_Avalonia project (in the same directory where CYaPass_Avalonia.csproj is found
4. That project simply allows CYaPass to copy the password to the clipboard.

##### Requires AES_Complete Library
1. There is now a dependency on the AES_Complete library which allows SiteKeys to be Encrypted/Decrypted and sent to WebAPI for storage
   - That project is located [here on github](https://github.com/raddevus/AES_Complete).
2. CYaPass only uses the DLL assembly from that project (though it does produce an EXE used for driving functionality / testing

## Backlog Items
-  [ - ] Remove hard-coded values, in ImportSiteKey method & read them from the user
     -  [ - ] demoKeys2022 - read maintoken from user
     -  [ - ] base url to LibreStore api (add config stored with the app)
-  [ - ] Calculate HMAC & compare to transfered value before attempting to decrypt data
-  [ - ] Change MaxLength control from TextBox to NumericUpDown
-  [ - ] Add NumericUpDown to Multi-hash
-  [ - ] Save SiteKeys as JSON in local file (make sure JSON format matches current CYaPass versions exactly)
-  [ - ] Fill SiteKeys list box from values read from local file (if exists)
-  [ - ] Implement Export SiteKeys button
-  [ - ] Implement Add Uppercase
-  [ - ] Implement Add Special Char
-  [ - ] Implement Max Length
-  [ - ] Clean up UI - various padding, margins etc.
-  [ - ] Implement Edit (SiteKey) button
-  [ - ] Implement Delete (SiteKey) button
