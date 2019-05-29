function msgBox(text) {
    window.external.MsgBox(text);
}

function callScript(type, file, fun, param, paramB) {
    return window.external.CallScript(type, file, fun, param, paramB);
}

function test() {
    var tx = callScript(1, "test", "测试", "测试");
    msgBox(tx);
}

function changeBrower() {
    window.external.ChangeBrower();
}

function openFileDialog(directory, defaultExt, filter) {
    if (!directory) { directory = "c:\\"; }
    if (!defaultExt) { defaultExt = ".xlsx" }
    if (!filter) { filter = "Microsoft Excel 97-2003文件(*.xls)|*.xls|Microsoft Excel文件(*.xlsx)|*.xlsx"; }
    return window.external.OpenFileDialog(directory, defaultExt, filter);
}

function saveFileDialog(directory, defaultExt, filter) {
    if (!directory) { directory = "c:\\"; }
    if (!defaultExt) { defaultExt = ".xlsx" }
    if (!filter) { filter = "Microsoft Excel 97-2003文件(*.xls)|*.xls|Microsoft Excel文件(*.xlsx)|*.xlsx"; }
    return window.external.SaveFileDialog(directory, defaultExt, filter);
}

function commonOpenFileDialog(directory) {
    if (!directory) { directory = "c:\\"; }
    return window.external.CommonOpenFileDialog(directory);
}

function loading()
{
    return window.external.Loading();
}

function preventSleep(includeDisplay) {
    if (includeDisplay == null)
    {
        includeDisplay = false;
    }
    return window.external.PreventSleep(includeDisplay);
}

function resotreSleep() {
    return window.external.ResotreSleep();
}