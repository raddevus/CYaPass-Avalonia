APP_NAME="CYaPass"
PUBLISH_DIR="bin/Release/net10.0/osx-arm64/publish"
APP_DIR="CYaPass.app"

rm -rf "$APP_DIR"
mkdir -p "$APP_DIR/Contents/MacOS"
mkdir -p "$APP_DIR/Contents/Resources"

cp Info.plist "$APP_DIR/Contents/Info.plist"
cp Assets/cyapass.icns "$APP_DIR/Contents/Resources/cyapass.icns"
cp -a "$PUBLISH_DIR"/. "$APP_DIR/Contents/MacOS/"
mv CYaPass.app/Contents/MacOS/CYaPass-Avalonia CYaPass.app/Contents/MacOS/CYaPass

