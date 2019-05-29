new Vue({
	el: '#app',
	data: function () {
		return {
			activeIndex: '1',
			activeIndex2: '1',
			about: false,
			useGuide: false,
			output: false,
			active: 1,
			url: "http://gd.10086.cn/estore/card/cardSearchTest.action",
			path: "D:\\",
			btnName: "立即导出",
			msg: "",
			times: 0,
			page: 0,
			dont: 0,
			form: {
				condId:"",
				condName:"",
				condValue:"",
				numTop:"",
				startNumTop:"",
				endNumTop:"",
				brand_zf:"",
				city_zf:"",
				brand:"",
				city:0,	//城市码
				numPeriod:0,	//号段
				feePeriod:"",
				numRule:0,	//号码规律码
				birthday:"",
				lastNumRule:"", //尾数规律码
				countNumRule:"",
				exceptNumRule:"",
				priceNumRule:"",
				taocan:"",
				cityCookie:"",
				queryType:"new",
				booknumber:"",
				ycmoney:"",
				tcmoney:"",
				brandName:"",
				cardPackSid:"",
				sortFiled:"",
				sortType:"",
				startNum:"",
				endNum:"",
				txtInput:"",
				cityNameTemp:"",
				cityCodeTemp:0,
				cardBrandTemp:"",
				cardBrandNameTemp:"",
				serialNum:"",
				hzhbjr:null,
				reqtaocan:null,
				reqcpcdtype:null,
				typecode:"",
				privcode:"",
				"cardpacks1.pageNo":1, //页码
				"cardpacks1.pageSize":15, //每页显示数量
				"cardpacks1.randomPageMapStr":"",
				"cardpacks.pageNo":1,
				"cardpacks.pageSize":15,
				"cardpacks.randomPageMapStr":"",
				isReRequest:false,
				_:""
			},
			pageCount: 0,
			page: 1,
			pageSize: 15,
			citys: [
					{ name: "广州", value: "200" },
					{ name: "东莞", value: "755" },
					{ name: "佛山", value: "769" },
					{ name: "汕头", value: "754" },
					{ name: "珠海", value: "756" },
					{ name: "惠州", value: "752" },
					{ name: "中山", value: "760" },
					{ name: "江门", value: "750" },
					{ name: "湛江", value: "759" },
					{ name: "揭阳", value: "663" },
					{ name: "茂名", value: "668" },
					{ name: "韶关", value: "751" },
					{ name: "清远", value: "763" },
					{ name: "肇庆", value: "758" },
					{ name: "云浮", value: "766" },
					{ name: "梅州", value: "753" },
					{ name: "河源", value: "762" },
					{ name: "潮州", value: "768" },
					{ name: "汕尾", value: "660" },
					{ name: "阳江", value: "662" },
			],
			period: [134,135,136,137,138,139,150,151,152,157,158,159,182,183,188],
			rules: [
					{ name: "AAA", value: "2" },
					{ name: "尾数88", value: "7" },
					{ name: "无4号码", value: "4" },
					{ name: "ABC", value: "9" },
					{ name: "ABAB", value: "3" },
					{ name: "AABB", value: "1" },
					{ name: "ABCD", value: "8" },
					{ name: "AABAA", value: "10" }
			]
		}
	},
	methods: {
		handleSelect: function (key, keyPath) {
			console.log(key, keyPath);
		},
		prev: function () {
			if (this.active-- < 0) {
				this.active = 0;
			}
		},
		next: function () {
			if (this.active++ > 2) {
				this.active = 0;
			}
		},
		openDy: function () {
			this.path = commonOpenFileDialog();
		},
		outOp: function ()
		{
			if (this.btnName == "导出中...") {
				this.dont = true;
			}
			else
			{
				this.times = 0;
				this.page = 0;
				this.active = 1;
				this.btnName = "立即导出";
			}
		},
		exportNum: function () {
			if (this.btnName == "导出中...") {
				callScript(1, "catch", "state", "stop", "");
				this.btnName = "已暂停";
				return;
			}
			else if (this.btnName == "已暂停")
			{
				callScript(1, "catch", "state", "start",  "");
				this.btnName = "导出中...";
				return;
			}
			this.btnName = "准备中...";
			var param = { url: this.url, form: this.form, page: this.page, pageSize: this.pageSize, pageCount: this.pageCount };
			var ret = callScript(1, "catch", "CheckLink", JSON.stringify(param), this.path);
			if (ret != null) {
				json = JSON.parse(ret);
				if (json.data != null) {
					this.page = json.data.page;
					var _this = this;
					ret = callScript(1, "catch", "GetNumber", JSON.stringify(param), this.path);
					if (ret.indexOf("执行中") != -1) {
						this.btnName = "导出中...";
						preventSleep(); //阻止休眠
						var timer = setInterval(
							function (e) {
								_this.times = loading();
								var t = parseInt(this.times / 1000);
								if (_this.times >= _this.page) {
									_this.btnName = "导出完成";
									resotreSleep(); //恢复休眠
									clearInterval(timer);
								}
							}, 5000);
					}
					else
					{
						this.msg = "计划执行失败！";
						this.output = true;
					}
				}
				else
				{
					this.msg = json.msg;
					this.output = true;
				}
			}
			else {
				this.msg = "导出失败！（原因：脚本错误）";
				this.output = true;
			}
		}
	},
	computed: {
		percentage: function ()
		{
			if (this.page == 0) {
				return 0;
			}
			else {
				return parseInt(this.times / this.page * 100);
			}
		}
	}
});