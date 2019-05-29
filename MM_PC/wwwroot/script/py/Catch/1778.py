#!/usr/bin/python
# -*- coding: utf-8 -*-

import json
import threading
import re
import time

#执行脚本
#fun: 函数名(字符串)
#tag: 标签(字符串)
#jsonStr: 请求参数(json格式字符串)
def Run(fun = None, paramA = None, paramB = None):
	Service.CleanupPy("catch\\catch")
	if(fun == "Index"):
		return Index(tag, jsonStr)
	elif(fun == "CheckLink"):
		return CheckLink(paramA, paramB)
	elif(fun == "GetNumber"):
		return GetNumber(paramA, paramB)
	elif(fun == "state"):
		return State(paramA)
	return None


# 获取号码
# url: 获取号码的链接
# ph：保存号码的路径
def State(paramA):
	global waiting
	if(paramA == "stop"):
		waiting = True
	else:
		waiting = False
	return "true"

# 检查路径和链接
# url: 获取号码的链接
# ph：保存号码的路径
def CheckLink(url, ph):
	if(Sdk.File.HasDir(ph) == False):
		return Data.ToRet("保存文件的路径错误！", "")
	html = Sdk.Http.Get(url)
	if(html == None or html == ""):
		return Data.ToRet("链接错误，或未连接网络！", "")
	n = html.find("|")
	page = int(html[:n])
	p = page / lh
	ct = lh * p #已获取的总页数
	yuShu = page - ct
	if(yuShu > 0):
		p += 1
	return Data.ToRet("", json.dumps({ "page": page, "count": p }))

# 获取号码
# url: 获取号码的链接
# ph：保存号码的路径
def GetNumber(url, ph):
	global pageCount
	global savePath
	global numSize
	global getUrl
	savePath = ph + '\\'
	Cache.Loading = 0
	rx = re.compile('page_no=[0-9]+&')
	getUrl = rx.sub('page_no={0}&', url)
	
	#获取链接页数
	html = Sdk.Http.Get(getUrl.replace('{0}', '1', 1))
	n = html.find("|")
	pageCount = int(html[:n])

	#开始多线程
	for i in range(threadNum):
		time.sleep(1)
		t = threading.Thread(target = GetNum, args=()) #多线程，传入参数0个
		t.setDaemon(True)
		t.start()
		threads.append(t)
	
	if(len(threads) > 0):
		s = threading.Thread(target = SaveNum, args=()) #多线程，传入参数0个
		s.setDaemon(True)
		s.start()
	return Data.ToRet("", '"执行中"')

def SaveNum():
	global waiting
	global numArr
	ph = savePath + "haoma_{0}.csv"
	while Cache.loading < pageCount:
		#time.sleep(60)
		if(len(numArr) > numSize):
			waiting = True
			arr = numArr[0 : numSize] #如果可以则导出最大数
			Sdk.Csv.Save(ph.replace("{0}", str(Sdk.Time.Stamp()), 1), json.dumps(arr))
			numArr = numArr[numSize:]
			waiting = False
	if(len(errorPage) > 0):
		for o in errorPage:
			GetNum_sub(o)
	if(len(numArr) > 0):
		pa = ph.replace("{0}", str(Sdk.Time.Stamp()), 1);
		bl = Sdk.Csv.Save(pa, json.dumps(numArr))
		if bl == False:
			Sdk.Log.WriteLine(Sdk.Csv.Ex)
	
# 获取号码
# ph：保存号码的路径
def GetNum():
	while Cache.loading < pageCount:
		if(waiting == False):
			Cache.loading += 1
			GetNum_sub(Cache.loading)

def GetNum_sub(page):
	url = getUrl.replace("{0}", str(page), 1)
	html = Sdk.Http.Get(url)
	bl = False
	if(html != '' and html != None):
		bl = GetNum_arr(html)
	if(bl == False):
		errorPage.append(Cache.loading)
	
# 获取号码
# url: 获取号码的链接
# ph：保存号码的路径
def GetNum_arr(html):
	global numArr
	bl = False
	idx = html.find("|")
	if(idx > 0):
		html = html[idx + 1:]
		if(html != None):
			narr = html.split('|')
			if(len(narr) > 0):
				for s in narr:
					if(s != ""):
						n = s.split(",")
						if(len(n) > 4):
							numArr.append({"号码":n[0], "卖价":n[3], "话费":n[4]})
							bl = True
	return bl

pageSize = 50 #单页数
lh = 800 # 多少页为一组
numSize = pageSize * lh #每组包含数

numArr = [] #号码数
errorPage = []
pageCount = 0
waiting = False

getUrl = "http://sz.1778.com/io/5.asp?cnt=50&page_no=2&numcategory=0&birth=&dis=6&lanmu=0&zifei=241&jiage=1"
savePath = "D:\\"

threads = [] #线程集合
threadNum = 2 #线程数