// Custom method for uploading a file, simulating SWFUpload
Selenium.prototype.doSwfUpload = function(selector, filename) {
 
	// Grab the SWFUpload element from the DOM
	var doc = this.browserbot.getCurrentWindow().document;
	var swf_uploader = Element.select($(doc.body), selector)[0];
 
	// Slurp the list of params from SWFUload, and parse them into a Hash
	var flashvars_s = swf_uploader.down('param[name=flashvars]').value;
	var flashvars = {};
	$A(flashvars_s.split(/&/)).each(function(kv){
		var key = unescape(kv.split(/=/)[0]);
		var value = unescape(kv.split(/=/)[1]);
		flashvars[key] = value;
	});
	var params_array = $A(decodeURI(flashvars.params).split(/&amp;/));
	var params = new Hash({});
	params_array.each(function(kv){
		var key = decodeURI(kv.split(/=/)[0]);
		var value = decodeURI(kv.split(/=/)[1]);
		params.set(key, value);
	});
	params.unset('format'); // Remove the format param, since we don't want to request as json
 
	// Grab the SWFUpload form from the hidden IFrame,
	// and insert the key/value params into the form
	var faker_form = Element.select($$('#selenium_fileupload_iframe')[0].contentDocument.body, '#swfupload_faker_form')[0];
	params.each(function(kv) {
		Element.insert(faker_form, { bottom: '<input type="hidden" name="'+kv.key+'" value="'+kv.value+'" />' });
	});
 
	// Assign the selected file to the file field
	netscape.security.PrivilegeManager.enablePrivilege("UniversalFileRead");
	this.browserbot.replaceText(faker_form.down('#swfupload_faker_file'), filename);
 
	// Assign the URL and submit the Form
	faker_form.action = flashvars.uploadURL;
	faker_form.submit();
 
	// Clean up the params
	Element.select(faker_form, 'input[type=hidden]').each(function(e){
		e.remove();
	});
 
	// Retarget the IFrame back to the fileupload frame
	$$('#selenium_fileupload_iframe')[0].contentWindow.location = "TestRunner-fileupload.html";
};

