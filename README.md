### Notes For Building
##### Requires (custom) TextCopy Library
1. There is now a dependency upon the (custom) TextCopy library which I've forked [here on github](https://github.com/raddevus/TextCopy).
2. The forked TextCopy project is updated to .NET 9.x but you can really only build the TextCopy.dll (TextCopy project) located under `/src/TextCopy`
   - I don't care about the blazor project or any of that other stuff - I had to fix it to work on other Linux distros
3. After you build the TextCopy project you need to A) create a directory named `/external` in the CYaPass_Avalonia project (in the same directory where CYaPass_Avalonia.csproj is found
4. That project simply allows CYaPass to copy the password to the clipboard.

