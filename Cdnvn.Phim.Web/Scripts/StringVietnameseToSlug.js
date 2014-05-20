jQuery.fn.stringVietNameseToSlug = function(options) {
	var defaults = {
		setEvents: 'keyup keydown blur', //set Events that your script will work
		getPut: '#permalink', //set output field
		space: '-', //Sets the space character. If the hyphen,
		prefix: '',
		suffix: '',
		replace: '', //Sample: /\s?\([^\)]*\)/gi
        vietnamese: false,
        random: false,
        auto: '',
		callback: false
	};

	var opts = jQuery.extend(defaults, options);

	jQuery(this).bind(defaults.setEvents, function () {
	   if(defaults.auto==''||$(defaults.auto).is(':checked'))
       {
		var text = jQuery(this).val();
		text = text.replace(defaults.replace, ""); //replace
		text = jQuery.trim(text.toString()); //Remove side spaces and convert to String Object
        text = text.toLowerCase();
        if(defaults.removeVN!=false)
        {
                text= text.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g,"a");  
                text= text.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g,"e");  
                text= text.replace(/ì|í|ị|ỉ|ĩ/g,"i");  
                text= text.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g,"o");  
                text= text.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g,"u");  
                text= text.replace(/ỳ|ý|ỵ|ỷ|ỹ/g,"y");  
                text= text.replace(/đ/g,"d");  
                text= text.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, defaults.space); 

        }
        
        stringToSlug = text.replace(/\s+/g, defaults.space);
		stringToSlug = stringToSlug.replace (new RegExp ('\\'+defaults.space+'{2,}', 'gmi'), defaults.space); // Remove any space character followed by Breakfast
		stringToSlug = stringToSlug.replace (new RegExp ('(^'+defaults.space+')|('+defaults.space+'$)', 'gmi'), ''); // Remove the space at the beginning or end of string
		
        if(defaults.random!=false)
        {
            var r = (((1 + Math.random()) * 0x10000) | 0).toString(16);
            stringToSlug = stringToSlug +defaults.space+ r;
            
        }
        
        stringToSlug = defaults.prefix.toLowerCase() + stringToSlug + defaults.suffix.toLowerCase();
        
		if(defaults.callback!=false){
			defaults.callback(stringToSlug);
		}

        jQuery(defaults.getPut).val(stringToSlug); //Write in value to input fields (input text, textarea, input hidden, ...)
		jQuery(defaults.getPut).html(stringToSlug); //Write in HTML tags (span, p, strong, h1, ...)
        
		return this;
        }
	});

  return this;
};