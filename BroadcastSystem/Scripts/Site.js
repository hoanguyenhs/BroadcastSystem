$(document).ready(function () {
	function startChange() {
		var startDate = start.value(),
			endDate = end.value();

		if (startDate) {
			startDate = new Date(startDate);
			startDate.setDate(startDate.getDate());
			end.min(startDate);
		} else if (endDate) {
			start.max(new Date(endDate));
		} else {
			endDate = new Date();
			start.max(endDate);
			end.min(endDate);
		}
	}

	function endChange() {
		var endDate = end.value(),
			startDate = start.value();

		if (endDate) {
			endDate = new Date(endDate);
			endDate.setDate(endDate.getDate());
			start.max(endDate);
		} else if (startDate) {
			end.min(new Date(startDate));
		} else {
			endDate = new Date();
			start.max(endDate);
			end.min(endDate);
		}
	}

	var today = kendo.date.today();
	try {
		var start = $("#start").kendoDateTimePicker({
			value: today,
			change: startChange,
			parseFormats: ["MM/dd/yyyy HH:mm:ss"]
		}).data("kendoDateTimePicker");

		var end = $("#end").kendoDateTimePicker({
			value: today,
			change: endChange,
			parseFormats: ["MM/dd/yyyy HH:mm:ss"]
		}).data("kendoDateTimePicker");

		start.max(end.value());
		end.min(start.value());

		$("#Content").kendoEditor({
			encoded: false,
			tools: [
				"bold",
				"italic",
				"underline",
				"strikethrough",
				"justifyLeft",
				"justifyCenter",
				"justifyRight",
				"justifyFull",
				"insertUnorderedList",
				"insertOrderedList",
				"indent",
				"outdent",
				"createLink",
				"unlink",
				"insertImage",
				"insertFile",
				"subscript",
				"superscript",
				"createTable",
				"addRowAbove",
				"addRowBelow",
				"addColumnLeft",
				"addColumnRight",
				"deleteRow",
				"deleteColumn",
				"viewHtml",
				"formatting",
				"cleanFormatting",
				"fontName",
				"fontSize",
				"foreColor",
				"backColor",
				"print"
			],
		});
	}
	catch (e) {
		console.log(e)
	}
});

function previewURL(text) {
	var originText = text;
	var isContaind = text.indexOf('<a href="');
	while (isContaind != -1) {
		text = text.substring(isContaind + 9, text.length - 1);
		var doubleQuote = text.indexOf('"');
		var url = text.substring(0, doubleQuote);
		if (url.indexOf("youtube") != -1) {
			var videoID = url.indexOf("?v=");
			videoID = url.substring(videoID + 3, url.length)
			var youtubeURL = "https://www.youtube.com/embed/" + videoID;
			var temp = url + '">' + url + '</a>';
			var appendIndex = originText.indexOf(temp);
			appendIndex = appendIndex + temp.length;
			originText = [originText.slice(0, appendIndex),
			"<br><iframe width='560' height='315' src='" + youtubeURL + "' frameborder='0' allowfullscreen></iframe>",
			//"<br><iframe id='frame' src='" + youtubeURL + "&output=embed'></iframe>",
			originText.slice(appendIndex)].join('');
			isContaind = text.indexOf('<a href="');
		} else {
			var temp = url + '">' + url + '</a>';
			var appendIndex = originText.indexOf(temp);
			appendIndex = appendIndex + temp.length;
			originText = [originText.slice(0, appendIndex),
			"<br><iframe id='frame' src='" + url + "&output=embed'></iframe>",
			originText.slice(appendIndex)].join('');
			isContaind = text.indexOf('<a href="');
		}
	}
	return originText;
}