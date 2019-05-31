/// 系统提示窗
/// text：显示内容
/// title：显示标题
function msgBox(text, title) {
    window.external.MsgBox(text, title);
}

/// 执行脚本
/// appName：应用名
/// fun：函数名
/// param1：参数1
/// param2：参数2
/// param3：参数3
/// 返回：执行结果
function callScript(appName, fun, param, paramB, paramC) {
    if (!param) { param = ""; }
    if (!paramB) { paramB = ""; }
    if (!paramC) { paramC = ""; }
    return window.external.CallScript(appName, fun, param, paramB, paramC);
}

/// 获取脚本执行错误
/// 返回：执行结果
function getError() {
    return window.external.getError();
}


/// 弹出打开文件框
/// directory：目录
/// defaultExt：拓展名
/// filter：过滤文件
function openFileDialog(directory, defaultExt, filter) {
    if (!directory) { directory = "c:\\"; }
    if (!defaultExt) { defaultExt = ".xlsx"; }
    if (!filter) { filter = "Microsoft Excel 97-2003文件(*.xls)|*.xls|Microsoft Excel文件(*.xlsx)|*.xlsx"; }
    return window.external.OpenFileDialog(directory, defaultExt, filter);
}

/// 弹出保存文件框
/// directory：目录
/// defaultExt：拓展名
/// filter：过滤文件
function saveFileDialog(directory, defaultExt, filter) {
    if (!directory) { directory = "c:\\"; }
    if (!defaultExt) { defaultExt = ".xlsx"; }
    if (!filter) { filter = "Microsoft Excel 97-2003文件(*.xls)|*.xls|Microsoft Excel文件(*.xlsx)|*.xlsx"; }
    return window.external.SaveFileDialog(directory, defaultExt, filter);
}

/// 设置文件框起始目录
/// directory：目录
function commonOpenFileDialog(directory) {
    if (!directory) { directory = "c:\\"; }
    return window.external.CommonOpenFileDialog(directory);
}

/// 加载进度
function loading() {
    return window.external.Loading();
}

/// 阻止系统休眠，用于长时间的加载时
function preventSleep(includeDisplay) {
    if (includeDisplay === null) {
        includeDisplay = false;
    }
    return window.external.PreventSleep(includeDisplay);
}

/// 恢复系统休眠
function resotreSleep() {
    return window.external.ResotreSleep();
}

/// 创建新窗口
/// url：网址
/// title：标题
function newWindow(url, title) {
    return window.external.NewWindow(url, title);
}

/// 测试调用脚本
function test() {
    var tx = callScript("./script/test.py", "测试", "测试", "测试");
    msgBox(tx, "测试脚本");
    //msgBox(getError(), "测试脚本");
    //newWindow("http://www.baidu.com", "超级美眉介绍");
}