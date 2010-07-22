			var swfDiv;
			function getID( swfID ){
				 if(navigator.appName.indexOf("Microsoft") != -1){
					  swfDiv = window[swfID];
				 }else{
					  swfDiv = document[swfID];
				 }
			}
			
			var qsParm = new Array();
			
			function qs() 
			{
				
				var query = window.location.search.substring(1);
				var parms = query.split('&');
				for (var i=0; i<parms.length; i++) 
				{
					var pos = parms[i].indexOf('=');
					if (pos > 0)
					{
						var key = parms[i].substring(0,pos);
						var val = parms[i].substring(pos+1);
						qsParm[key] = val;
					}
				}
			} 
			function DoLoadURL() 
			{
				
				getID('test3d');
				
				qsParm['URL'] = null;
				qs();
				
				if(qsParm['URL'] == null)
				{
						alert("NO URL!");
				}else
				{
					swfDiv.Load(qsParm["URL"]);
				}
			} 