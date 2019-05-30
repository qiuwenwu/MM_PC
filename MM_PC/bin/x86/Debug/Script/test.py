#!/usr/bin/python
# -*- coding: utf-8 -*-

import json
import threading
import re
import time
import urllib
import httplib

#执行脚本
#fun: 函数名(字符串)
#tag: 标签(字符串)
#jsonStr: 请求参数(json格式字符串)
def Main(fun = None, paramA = None, paramB = None, paramC = None):
	Engine.Unload("./test.py")
	log = SDK.Log()
	log.WriteLine("1测试启动")
	return "1231231231"