o3djs.require('o3djs.util');
o3djs.require('o3djs.math');
o3djs.require('o3djs.quaternions');
o3djs.require('o3djs.primitives');
o3djs.require('o3djs.rendergraph');
o3djs.require('o3djs.pack');
o3djs.require('o3djs.arcball');
o3djs.require('o3djs.scene');
o3djs.require('o3djs.io');
o3djs.require('o3djs.canvas');
o3djs.require('o3djs.picking');

var g_o3d;
var g_math;
var g_quaternions;
var g_client = null;
var g_aball;
var g_thisRot;
var g_lastRot;
var g_pack = null;
var g_ModelPack = null;
var g_BackupEffects = Array();
var g_mainPack;
var g_viewInfo;
var g_lightPosParam;
var g_loadingElement;
var g_o3dWidth = -1;
var g_o3dHeight = -1;
var g_infullScreen = false;
var g_o3dElement;
var g_o3dPrimitives;
var g_model_min;
var g_finished = false;                     // for selenium
var g_camvec = [5, 5, 5];                   //the offset of the camera from the camera's center of rotation
var g_camcenter = [0, 0, 0];                //the cameras center of rotation
var g_oldx = 0;                             //previous mouse x
var g_oldy = 0;                             //previous mouse y
var g_moving = false;                       //flag to store wheather or not key is down
var g_mouseRotateSensitivity = 1 / 250;     //The sencetivity to the mouse movement
var g_mouseMoveSensitivity = 1 / 350;
var g_defaultRadius = 1;                    //the radius of the bounding sphere
var g_modelCenter = [0, 0, 0];              //center of the model
var g_camcenterGoal = [0, 0, 0];            //goal for the center animation
var g_camvecGoal = [5, 5, 5];               //goal for the cam vec animation
var g_Animating = false;                    //are we animating?
var g_modelSize = 0;                        //the radius of the model
var g_camera = { farPlane: 5000, nearPlane: 0.1 };
var g_textCanvas;  
var g_paint;  
var g_canvasLib;                            //the transform for the grid
var g_sampler;                              //the sampler for the grid texture
var g_grid;                                 
var g_unitscale;                            //the unit scale to set after the model loads
var g_upaxis;                               //the upaxis to set after the model loads
var g_currentupaxis;                        //the current up orientation
var g_WireFrame;                        //the current up orientation
//vectors used for camera model
var sidevec =  [1, 0, 0];   
var frontvec = [0, 0, 1];
var upvec =    [0, 1, 0];
var g_sceneRoot;
var g_hudRoot;
var g_hudViewInfo;
var g_dragging = false;                     //are we dragging?
var nextrot = 90;                           //whats the next rotation when Z->X
var g_Hudtest;
var g_logo;
var g_shadowQuad = null;
var g_GUIarray = [];

var g_ThumbArray = [];

var g_TextureCache = {};

var g_TextureThumbArray = [];
var bbox;
var g_fullscreenButton = null;
var g_init = false;
var oldHit;
//swap the side and up vectors
function swapFrontUp() {

   
    var temp = frontvec;
    frontvec = upvec;
    upvec = temp;

    if(upvec.y == 1)
    {
        g_currentupaxis == 'Y';
    }
    
    if(upvec.z == 1)
    {
        g_currentupaxis == 'Z';
    }

    //build a quat to rotate the grid
    var rot = g_quaternions.axisRotation(sidevec, g_math.degToRad(nextrot));
    g_grid.SetMatrix(g_quaternions.quaternionToRotation(rot));
    
    //setup the inverse rotation for next click
    if (nextrot == 90) {
        nextrot = 0;
        //center up the grid in the new coords
        g_grid.SetPosition(g_modelCenter[0], g_modelCenter[1], g_model_min[2] + .01, 0);
        //remove the shadow quad from the graph
        g_shadowQuad.transform.parent = null;
        //create a new shadow quad with the dimentions of the bounding box
        g_shadowQuad = new HUDQuad('Images/shadow3.png', 0, 0, (bbox.minExtent[1] - bbox.maxExtent[1]) *1.5, (bbox.minExtent[0] - bbox.maxExtent[0])*1.5, g_viewInfo, g_sceneRoot, 1,false);
        g_shadowQuad.ResetTransforms();
        //rotate the shadow to the same rot as the grid
        g_shadowQuad.SetMatrix(g_quaternions.quaternionToRotation(rot));
        //center it up under the model
        g_shadowQuad.SetPosition(g_modelCenter[0], g_modelCenter[1],bbox.minExtent[2] +.01, 0);
        g_shadowQuad.action = function() { };
        //No Zwrite for the shadow, since it's transparent
        g_shadowQuad.material.state.getStateParam('ZWriteEnable').value = false;
        g_grid.material.state.getStateParam('ZWriteEnable').value = false;
        //Swap the camera directions so the camera jumps less
        var temp3 = g_camvec[1];
        g_camvec[1] = g_camvec[2];
        g_camvec[2] = temp3;
        g_camvec[0] = g_camvec[0] * -1;
    }
    else {
        nextrot = 90;
        //center up the grid in the new coords
        g_grid.SetPosition(g_modelCenter[0], g_model_min[1], g_modelCenter[2], 0);
        //remove the shadow quad from the graph
        g_shadowQuad.transform.parent = null;
        //create a new shadow quad with the dimentions of the bounding box
        g_shadowQuad = new HUDQuad('Images/shadow3.png', 0, 0, (bbox.minExtent[2] - bbox.maxExtent[2])*1.5, (bbox.minExtent[0] - bbox.maxExtent[0])*1.5, g_viewInfo, g_sceneRoot, 1,false);
        g_shadowQuad.ResetTransforms();
        //rotate the shadow to the same rot as the grid
        g_shadowQuad.SetMatrix(g_quaternions.quaternionToRotation(rot));
        //center it up under the model
        g_shadowQuad.SetPosition(g_modelCenter[0], bbox.minExtent[1] + .01, g_modelCenter[2], 0);
        g_shadowQuad.action = function() { };
        //No Zwrite for the shadow, since it's transparent
        g_shadowQuad.material.state.getStateParam('ZWriteEnable').value = false;
        //Swap the camera directions so the camera jumps less
        var temp3 = g_camvec[1];
        g_camvec[1] = g_camvec[2];
        g_camvec[2] = temp3;
        g_camvec[0] = g_camvec[0] * -1;
    }
    updateCamera();
}


function drawText(str) {  
    // Clear to completely transparent.  
    g_textCanvas.canvas.clear([0, 0, 0, 0]);  
      
    // Reuse the global paint object  
    var paint = g_paint;  
    paint.color = [0, 0, 0, 1];  
    paint.textSize = 12;  
    paint.textTypeface = 'Comic Sans MS';  
    paint.textAlign = g_o3d.CanvasPaint.LEFT;  
    paint.shader = null;  
    g_textCanvas.canvas.drawText(str, 10, 15, paint);  
       
    g_textCanvas.updateTexture();  
} 

//animate to the front view
function viewFront() {
    g_camvecGoal = g_math.mulVectorScalar(frontvec, g_defaultRadius);
    g_camcenterGoal = g_modelCenter;
    g_Animating = true;
}

//animate to the side view
function viewSide() {
    g_camvecGoal = g_math.mulVectorScalar(sidevec, g_defaultRadius);
    g_camcenterGoal = g_modelCenter;
    g_Animating = true;
}

//animate to the top view
function viewTop() {
    g_camvecGoal = g_math.addVector(g_math.mulVectorScalar(upvec, g_defaultRadius),g_math.mulVectorScalar(sidevec, .01));
    g_camcenterGoal = g_modelCenter;
    g_Animating = true;
}
//A timer to animate the camera position
function Animate() {
    if (g_Animating == true) {
        //interpolate the camera toward the goal
        g_camvec = g_math.lerpVector(g_camvec, g_camvecGoal, .03);
        g_camcenter = g_math.lerpVector(g_camcenter, g_camcenterGoal, .03);
        updateCamera();

    }
    //repeat 30 times a second
    var t = setTimeout("Animate()", 33);
}

//When a button is presssed, print a message and set flags
function startDragging(e) {
    g_lastRot = g_thisRot;
    if (e.button == 0) {
        g_dragging = true;
        drawText("Drag to Rotate");
    }
    if (e.button == 1) {
        g_moving = true;
        drawText("Drag to Move");
    }
}

//when a key is held, set the moving flag. This prevents rotation and causes mouse motion to be interpreted as movement
function keyDown(e) {

    //cancel animation
    g_Animating = false;
    g_camvec = [g_camvec[0], g_camvec[1], g_camvec[2]];
    
    //get the size of the object, and use that to determine how sensitive the keys are
    var diag = g_math.length(g_math.subVector(bbox.maxExtent,
                                                bbox.minExtent)) / 150;
    //A vector to hold the motion                                                
    var camoffset = [0, 0, 0];
    
    //If the the up or W key is pressed
    if (o3djs.event.getEventKeyChar(e) == 87 || o3djs.event.getEventKeyChar(e) == 38) {
        camoffset = [0, 0, -diag];
        drawText("Move Forward");
    }
    //if the down or S key is pressed
    if (o3djs.event.getEventKeyChar(e) == 83 || o3djs.event.getEventKeyChar(e) == 40) {
        camoffset = [0, 0, diag];
        drawText("Move Backward");
    }
    //if the left or A key is pressed
    if (o3djs.event.getEventKeyChar(e) == 65 || o3djs.event.getEventKeyChar(e) == 37) {
        camoffset = [-diag, 0, 0];
        drawText("Move Left");
    }
    //if the right or D key is pressed
    if (o3djs.event.getEventKeyChar(e) == 68 || o3djs.event.getEventKeyChar(e) == 39) {
        camoffset = [diag, 0, 0];
        drawText("Move Right");
    }    
    //transform the offset from screen to world space
    camoffset = g_math.mulVectorMatrix(camoffset, g_math.inverse(g_viewInfo.drawContext.view));
    //add the offset to the camera center of rotation
    g_camcenter = g_math.addVector(g_camcenter, camoffset);
    updateCamera();
}
//switch back to rotation when the key is released
function keyUp(e) {
    //g_moving = false;
    drawText("");
}

function RotateCamera(relx, rely) {
    
    //The up axis - this math won't allow the camera to roll
    var axis = upvec;

    //create a quat based on the relitive mouse movement
    var rot = g_quaternions.axisRotation(axis, -relx * g_mouseRotateSensitivity / g_modelSize * g_math.length(g_camvec));
    var mat = g_quaternions.quaternionToRotation(rot);
    //mul the offset vector by the quat
    g_camvec = g_math.mulVectorMatrix(g_camvec, mat);
    //normalize the camera offset vector
    var camvecnormalized = g_math.normalize(g_camvec);
    //cross with the up vector to get the current side vector
    var sidevec = g_math.cross(axis, camvecnormalized);
    //make a quat to rotate around the new side vector based on the relative mouse y
    var rotside = g_quaternions.axisRotation(sidevec, -rely * g_mouseRotateSensitivity / g_modelSize * g_math.length(g_camvec));
    var matside = g_quaternions.quaternionToRotation(rotside);
    //mul the camera offset vec by the side vector quat
    g_camvec = g_math.mulVectorMatrix(g_camvec, matside);
   
    return;
}

function drag(e) {

   

    //if the mouse is not down, then clear the text
    if (g_dragging == false && g_moving == false) {
        drawText("");
    } 
    //check the mouse over the GUI
	mouseOut(e);
	mouseOver(e);
	//mouseMove(e);
	
    //subtract the old mouse position from the new one to get the relative motion
    var relx = e.x - g_oldx;
    var rely = e.y - g_oldy;
    g_oldx = e.x;
    g_oldy = e.y;

    if (mouseDrag(e,relx, rely)) return;

    //cancel animation if the user drags or moves the mouse
    if (g_dragging == true || g_moving == true) {
        if (g_Animating == true) {
            g_Animating = false;
            //This is tricky. Something g_math is doing is making camvec a 4d vector
            //which then screws up the math. This kills the 4
            g_camvec = [g_camvec[0], g_camvec[1], g_camvec[2]];
           
        }
    }

    //if we're dragging the mouse, but not holding a key
    if (g_dragging && !g_moving) {

        var oldvec = g_camvec;
        RotateCamera(relx, rely);

        //if the camera vector is to near the upvector, reset it and repeat the 
        //motion but without the Y mouse motion
        if (Math.abs(g_math.dot(upvec, g_math.normalize(g_camvec))) > .95) {
            g_camvec = oldvec;
            RotateCamera(relx, 0);
        }

    }
    //If a key is held, move instead of rotating
    if (g_moving) {
    
        //tranform the relitive mouse movement from view space to world space, then add to the camera position
        var camoffset = [-relx * g_mouseMoveSensitivity, rely * g_mouseMoveSensitivity, 0];
        camoffset = g_math.mulVectorScalar(camoffset, g_math.length(g_camvec));
        camoffset = g_math.mulVectorMatrix(camoffset, g_math.inverse(g_viewInfo.drawContext.view));
        g_camcenter = g_math.addVector(g_camcenter, camoffset);
    }
    updateCamera();
}

//Loop over each GUI object in the GUI array, and hittest with the mouse coords
function pick(e) {
    for (i = 0; i < g_GUIarray.length; i++) {
        if (g_GUIarray[i].hittest(e.x, e.y))
            g_GUIarray[i].action();
    }

}
//Loop over each GUI object in the GUI array, and hittest with the mouse coords
function mouseOver(e) {
    for (i = 0; i < g_GUIarray.length; i++) {
        if (g_GUIarray[i].hittest(e.x, e.y))
            g_GUIarray[i].mouseOver();
    }
}
//Loop over each GUI object in the GUI array, and hittest with the mouse coords
function mouseOut(e) {
    for (i = 0; i < g_GUIarray.length; i++) {
        if (!g_GUIarray[i].hittest(e.x, e.y))
            g_GUIarray[i].mouseOut();
    }
}
//Loop over each GUI object in the GUI array, and hittest with the mouse coords
function mouseMove(e) {
    for (i = 0; i < g_GUIarray.length; i++) {
        if (!g_GUIarray[i].hittest(e.x, e.y))
            g_GUIarray[i].mouseMove(e.x,e.y);
    }
}
function mouseDrag(e,relx, rely) {
    var hit = false;
    for (i = 0; i < g_GUIarray.length; i++) {
        if (g_GUIarray[i].hittest(e.x, e.y) && g_dragging) {
            
            hit = hit || g_GUIarray[i].mouseDrag(relx, rely);
        }
    }
    return hit;
}
//Stop draging the mouse
function stopDragging(e) {
    g_dragging = false;
    g_moving = false;
    drawText("");
}
//Rebuild the View Matrix
function updateCamera() {

    g_camera.eye = g_math.addVector(g_camcenter, g_camvec);
    g_camera.target = g_camcenter;

    g_viewInfo.drawContext.view = g_math.matrix4.lookAt(g_camera.eye,
                                                      g_camera.target,
                                                      upvec);
    g_lightPosParam.value = g_camera.eye;
   
}
//Rebuild the projection matrix
function updateProjection() {
    // Create a perspective projection matrix.
    g_viewInfo.drawContext.projection = g_math.matrix4.perspective(
    g_math.degToRad(45), g_o3dWidth / g_o3dHeight, g_camera.nearPlane,
    g_camera.farPlane);

    g_hudViewInfo.drawContext.projection = g_math.matrix4.orthographic(
               0 + 0.5,
               g_client.width + 0.5,
               g_client.height + 0.5,
               0 + 0.5,
               0.001,
               1000);
}
//Mult the camvec by the a scalar depending on the mouse wheel
function scrollMe(e) {
    if (e.deltaY) {
        var t = 1;
        if (e.deltaY > 0)
            t = 11 / 12;
        else
            t = 13 / 12;

        //if animating, then cancel the animation
        if (g_Animating == true) {
            g_Animating = false;
            //Make sure that the camvec stays a 3D vec. Something is making it 4D
            g_camvec = [g_camvec[0], g_camvec[1], g_camvec[2]];
        }
        
        //Lengthen or shorten the vector
        g_camvec = g_math.mulVectorScalar(g_camvec, t);

        updateCamera();
    }
}

//A function object that can be associated with a texture load
//Stores the sampler to attach the texture to on load complete
function TextureLoadCallbackObject(sampler,infilename) {
    var sampler2 = sampler;
    var filename = infilename;
    this.callback = function (texture, exception) {
        if (exception) {
            sampler2.texture = null;

        } else {
            sampler2.texture = texture;
            g_TextureCache[filename] = texture;
        }
    }
}
function GetSolidEffect(material) {

    var path = window.location.href;
    var index = path.lastIndexOf('/');
    var path2 = path.substring(0, index + 1);
    var index2 = path2.lastIndexOf('/');
    var path3 = path2.substring(0, index2);
    var index3 = path3.lastIndexOf('/');

    
    var shaderString = path3.substring(0, index3 + 1) + 'Scripts/wire.shader';
    effect = g_pack.createObject('Effect');
    o3djs.effect.loadEffect(effect, shaderString);
    material.effect = effect;
    material.drawList = g_viewInfo.zOrderedDrawList;
    effect.createUniformParameters(material);
}
//An object to encapsulate the functionality to draw a quad on the screen
//and test the mouse for hit
//TODO:Mouseover, Mouseleave
function HUDQuad(filename, x, y, height, width, viewinfo, parent, tile, gui) {

  
        //some manipulation on the filename to get the absolute path
        var path = window.location.href;
        var index = path.lastIndexOf('/');
        var path2 = path.substring(0, index + 1);
        var index2 = path2.lastIndexOf('/');
        var path3 = path2.substring(0, index2);
        var index3 = path3.lastIndexOf('/');

      if(filename != '')
    {
        filename = path3.substring(0, index3 + 1) + filename;
    }
    
    this.filename = filename;
    this.x = x;
    this.y = y;
    this.z = 0;
    this.height = height;
    this.width = width;
    //Create a transform and attach it to the scenegraph
    this.transform = g_pack.createObject('Transform');
    this.transform.parent = parent;
    //Create a material
    this.material = g_pack.createObject('Material');
    //Load the shader
    var shaderString = path3.substring(0, index3 + 1) + 'Scripts/grid7.shader';
    //create the effect
    this.effect = g_pack.createObject('Effect');
    o3djs.effect.loadEffect(this.effect, shaderString);
    this.material.effect = this.effect;
    this.material.drawList = viewinfo.zOrderedDrawList;
    this.effect.createUniformParameters(this.material);

    //Attach the texture to the sampler in the shader
    this.samplerParam = this.material.getParam('texSampler0');
    this.sampler = g_pack.createObject('Sampler');
    this.sampler.minFilter = g_o3d.Sampler.ANISOTROPIC;
    this.sampler.maxAnisotropy = 4;
    this.samplerParam.value = this.sampler;

    //This param controls how many times the texture is tiles on 
    //this quad
    this.transform.createParam('tile', 'ParamFloat').value = tile;
    this.alpha = this.transform.createParam('alpha', 'ParamFloat');
    this.alpha.value = 1;
    
    //Create the textureload callback for this sampler
    this.callback = new TextureLoadCallbackObject(this.sampler, filename);
   
    //create a stateset to hold the rendering state for this node
   var myState = g_pack.createObject('State');
     
    // then set the states you want. For typical alpha blending
    myState.getStateParam('AlphaBlendEnable').value = true;
    myState.getStateParam('SourceBlendFunction').value =
    g_o3d.State.BLENDFUNC_SOURCE_ALPHA;
    myState.getStateParam('DestinationBlendFunction').value =
    g_o3d.State.BLENDFUNC_INVERSE_SOURCE_ALPHA;
    
    this.material.state = myState;
    //load the texture, and on callback completed assign to sampler
    if(this.filename != "")
        o3djs.io.loadTexture(g_pack, filename, this.callback.callback);
    this.material.state = myState;
    //create a plane with the grid material
    if(!gui)
        this.shape = g_o3dPrimitives.createPlane(g_pack, this.material, width, height, 1, 1, null);
    else
        this.shape = g_o3dPrimitives.createPlane(g_pack, this.material, 1, 1, 1, 1, null);
    //this.shape = g_o3dPrimitives.createCube(g_pack, this.material, 400);
    this.shape.createDrawElements(g_pack, null);

    //add the grid shape to the transform
    this.transform.addShape(this.shape);
    
    
    //the default rotation is facing the camera of the HUD
    var rot =g_quaternions.axisRotation([-1, 0, 0], g_math.degToRad(90));
    this.transform.localMatrix = g_quaternions.quaternionToRotation(rot);
    this.transform.localMatrix = g_math.matrix4.setTranslation(this.transform.localMatrix, [0, 0, 0, 0]);
    if(gui)
        this.transform.localMatrix = g_math.matrix4.scale(this.transform.localMatrix, [width, 1, height]);

    this.SetAlpha = function (a) {
        this.alpha.value = a;
    }
    this.SetScale = function (x, y) {
        this.transform.localMatrix = g_math.matrix4.scale(this.transform.localMatrix, [1 / this.width,1, 1 / this.height]);
        this.width = x;
        this.height = y;
        this.transform.localMatrix = g_math.matrix4.scale(this.transform.localMatrix, [this.width,1, this.height]);
    }

    //The set position function
    this.SetPosition = function (x, y, z, w) {
        this.transform.localMatrix = g_math.matrix4.setTranslation(this.transform.localMatrix, [x, y, z, w]);
        this.x = x;
        this.y = y;
        this.z = z;
    }
    //Set the default position to the view plane
    this.SetPosition(x, y, .1, 0);
    
    //A function to set the transform matrix
    this.SetMatrix = function(matrix) {
        this.transform.localMatrix = matrix;
    }
    
    //A function to reset the transform matrix
    this.ResetTransforms = function() {
        this.transform.localMatrix = new g_math.identity(4);
    }    
    //The default action. This will be called when the HitTest is true
    this.action = function() {
        //alert("Hit!");
    }
    this.mouseMove = function (x,y) {
        //alert("Hit!");
    }
    this.mouseDrag = function (x, y) {
        //alert("Hit!");
        return false;
    }
    this.mouseOver = function() {
        //alert("Hit!");
    }
	this.mouseOut = function() {
        //alert("Hit!");
    }
    
    //the hittest functions. Detect if the coords are inside of the rect or not
    //If they are, then call action()
    this.hittest = function(x, y) {
        x += this.width / 2;
        y += this.height / 2;
        if (x > this.x && x < this.x + this.width)
            if (y > this.y && y < this.y + this.height)
            return true;
        return false;
    }
    this.SwapImage = function (filename) {
        if (this.filename == filename)
            return;
        var path = window.location.href;
        var index = path.lastIndexOf('/');
        var path2 = path.substring(0, index + 1);
        var index2 = path2.lastIndexOf('/');
        var path3 = path2.substring(0, index2);
        var index3 = path3.lastIndexOf('/');

        filename = path3.substring(0, index3 + 1) + filename;

        this.filename = filename;

        if (g_TextureCache[filename] != undefined) {
            this.SetTexture(g_TextureCache[filename]);
        } else {

            this.callback = new TextureLoadCallbackObject(this.sampler,filename);
            o3djs.io.loadTexture(g_pack, filename, this.callback.callback);
        }
    }
    this.SetTexture = function (texture) {
       
       this.sampler.texture = texture;
        
    }

}
function ajaxImageSend(path, params) {
   
    var xhr;
    try { xhr = new ActiveXObject('Msxml2.XMLHTTP'); }
    catch (e) {
        try { xhr = new ActiveXObject('Microsoft.XMLHTTP'); }
        catch (e2) {
            try { xhr = new XMLHttpRequest(); }
            catch (e3) { xhr = false; }
        }
    }

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            if (xhr.status == 200) {
                document.getElementById("ctl00_ContentPlaceHolder1_ScreenshotImage").src = path + "?Session=true";
            }
            else
                alert("Error code " + xhr.status);
        }
    };

    xhr.open("POST", path+ "?ContentObjectID=adl~70",true);
    xhr.send(params); 

}

//enable wireframe rendering
function WireFrameOn()
{
    var materials = g_ModelPack.getObjectsByClassName('o3d.Material');
            for (var m = 0; m < materials.length; ++m) {
                var material = materials[m];

                var myState = g_pack.createObject('State');

                // then set the states you want. For typical alpha blending
                myState.getStateParam('AlphaBlendEnable').value = false;
                myState.getStateParam('FillMode').value =  g_o3d.State.WIREFRAME;
                material.state = myState;
                material.createParam("backupeffect", 'ParamEffect');
                material.getParam("backupeffect").value = material.effect;
                GetSolidEffect(material);
            }
}
//enable normal rendering
function WireFrameOff() {

    
     var materials = g_ModelPack.getObjectsByClassName('o3d.Material');
     for (var m = 0; m < materials.length; ++m) {
         var material = materials[m];
         if (material.getParam("backupeffect") != null) {

             var myState = g_pack.createObject('State');

             // then set the states you want. For typical alpha blending
             myState.getStateParam('AlphaBlendEnable').value = false;
             myState.getStateParam('FillMode').value = g_o3d.State.SOLID;
             material.state = myState;

             material.effect = material.getParam("backupeffect").value;
         }
     }
}

//Toggle wireframe rendering
function ToggleWireFrame() {

    g_WireFrame = g_WireFrame == false;
    if (g_WireFrame)
        WireFrameOn();
    else
        WireFrameOff();
}


function screenshot() {

    alert("taking screenshot");
    var qsParm = new Array();
    qsParm["imagedata"] = shot;
    var backupmatrix = g_hudRoot.localMatrix;
    g_hudRoot.localMatrix = g_math.matrix4.setTranslation(this.transform.localMatrix, [1000, 1000, 1000, 1000]);
    g_client.render();
    var shot = g_client.toDataURL();
    ajaxImageSend("ScreenShot.ashx", shot);
    g_hudRoot.localMatrix = backupmatrix;
   
}
// build the gui quads
function BuildHUD() {

    //the top button
    var top = new HUDQuad('Images/Icons/3dr_btn_T_cube.png', 10, 10, 20, 20, g_hudViewInfo, g_hudRoot, 1,true);
    g_GUIarray[g_GUIarray.length] = top;
    top.action = viewTop;
    top.mouseOver = function() { drawText("Top"); top.SwapImage('Images/Icons/3dr_btn_T_grey_cube.png') };
	top.mouseOut = function(){ top.SwapImage('Images/Icons/3dr_btn_T_cube.png') };
    //the left button
	var left = new HUDQuad('Images/Icons/3dr_btn_L_cube.png', 35, 10, 20, 20, g_hudViewInfo, g_hudRoot, 1, true);
    g_GUIarray[g_GUIarray.length] = left;
    left.action = viewSide;
    left.mouseOver = function() { drawText("Side"); left.SwapImage('Images/Icons/3dr_btn_R_grey_cube.png')  };
	left.mouseOut = function(){ left.SwapImage('Images/Icons/3dr_btn_L_cube.png') };
    //the side button
	var front = new HUDQuad('Images/Icons/3dr_btn_R_cube.png', 60, 10, 20, 20, g_hudViewInfo, g_hudRoot, 1, true);
    g_GUIarray[g_GUIarray.length] = front;
    front.action = viewFront;
    front.mouseOver = function() { drawText("Front"); front.SwapImage('Images/Icons/3dr_btn_L_grey_cube.png')  };
	front.mouseOut = function(){ front.SwapImage('Images/Icons/3dr_btn_R_cube.png') };
    //the swap up axis button
	var swap = new HUDQuad('Images/Icons/3dr_btn_y.png', 85, 10, 20, 20, g_hudViewInfo, g_hudRoot, 1, true);
    g_GUIarray[g_GUIarray.length] = swap;
    swap.action = swapFrontUp;
    swap.mouseOver = function () { drawText("Swap Up Vector"); swap.SwapImage('Images/Icons/3dr_btn_grey_y.png')  };
	swap.mouseOut = function(){ swap.SwapImage('Images/Icons/3dr_btn_y.png') };

	var wireframe = new HUDQuad('Images/Icons/3dr_btn_blue_wireframe.png', 110, 10, 20, 20, g_hudViewInfo, g_hudRoot, 1, true);
    g_GUIarray[g_GUIarray.length] = wireframe;
    wireframe.action = ToggleWireFrame;
    wireframe.mouseOver = function () { drawText("WireFrame"); wireframe.SwapImage('Images/Icons/3dr_btn_blue_wireframe.png') };
    wireframe.mouseOut = function () { wireframe.SwapImage('Images/Icons/3dr_btn_blue_wireframe.png') };



    //disabled until the screenshot stuff is done!
    //var ss = new HUDQuad('Images/Icons/6.png', 135, 15, 30, 30, g_hudViewInfo, g_hudRoot, 1);
    //g_GUIarray[g_GUIarray.length] = ss;
    //ss.action = screenshot;
    //ss.mouseOver = function () { drawText("Take a screen shot") };
    
    //the fullscreen button. This is in a globab var so it  can be moved on client resize
	g_fullscreenButton = new HUDQuad('Images/Icons/3dr_btn_expand.png', g_o3dWidth - 10, 10, 20, 20, g_hudViewInfo, g_hudRoot, 1, true);
    g_GUIarray[g_GUIarray.length] = g_fullscreenButton;
    g_fullscreenButton.action = function () {

        if (g_infullScreen == true) {
           
            g_client.cancelFullscreenDisplay();
        } 


    };
    g_fullscreenButton.mouseOver = function() { drawText("FullScreen"); g_fullscreenButton.SwapImage('Images/Icons/3dr_btn_grey_expand.png') };
	g_fullscreenButton.mouseOut = function(){ g_fullscreenButton.SwapImage('Images/Icons/3dr_btn_expand.png') };

	o3djs.event.addEventListener(g_o3dElement, 'resize', handleResizeEvent);

}

function handleResizeEvent(event) {
    // Only show the fullscreen banner if we're in plugin mode.
    g_infullScreen = event.fullscreen;
}


function enableInput(enable) {
   
}

function SetScale( node,  scale) {
  //  if(node)
   // node.scale = scale;
}
function SetAxis( axis) {
    if(axis == 'Y' && g_currentupaxis == 'Z') {
        
        swapFrontUp();
    }
    if(axis == 'Z' && g_currentupaxis == 'Y') {
        
        swapFrontUp();
    }
}

function getClassName(obj) {
    // get classname abstracted from
    // constructor property
    var c = obj.constructor.toString();
    var start = c.indexOf('function ') + 9;
    var stop = c.indexOf('(');
    c = c.substring(start, stop);
    return c;
}
function isImageFile(type) {
    if (type == 'jpg' || type == 'Jpg' || type == 'JPG')
        return true;
    if (type == 'gif' || type == 'Gif' || type == 'GIF')
        return true;
    if (type == 'png' || type == 'Png' || type == 'PNG')
        return true;
    if (type == 'tga' || type == 'Tga' || type == 'TGA')
        return true;
    if (type == 'dds' || type == 'Dds' || type == 'DDS')
        return true;
    return false;
}
//create thumbnail quads for the textures in the archive
function ShowTextureThumbs(path) {

    //after the file is loaded
    function callback(archiveInfo, exception) {
        if (!exception) {


            var files = archiveInfo.files;
            var count = 1;

            for (var key in files) {

                if (isImageFile(key.substr(key.length - 3))) {
                    count++;
                }
            }
            var i = 1;

            var bigthumbborder = new HUDQuad('Images/Icons/Thumbborder.png', g_client.width / 2, g_client.width / 2, g_client.width , g_client.width, g_hudViewInfo, g_hudRoot, 1, true);
            g_GUIarray[g_GUIarray.length] = bigthumbborder;
            bigthumbborder.hide = function () {
                this.SetPosition(g_client.width * 3, g_client.height * 3, -2, 1);
                this.SetScale(g_client.width / 1.25, g_client.width / 1.25);
            }
            bigthumbborder.show = function () {
                this.SetPosition(g_client.width / 2, g_client.height / 2, -2, 1);
                this.SetScale(g_client.width / 1.25, g_client.width / 1.25);
            }

            bigthumbborder.hide();

            for (var key in files) {
                
                if (isImageFile(key.substr(key.length - 3))) {

                    var locali = i;
                    var newtexture = o3djs.texture.createTextureFromRawData(g_pack, files[key], false, true, 256, 256);


                    var yspace = g_client.height / count;
                    if (yspace < 70)
                        yspace = 70;

           
                    var newthumbback = new HUDQuad('', 40, yspace * locali, 60, 60, g_hudViewInfo, g_hudRoot, 1, true);
                    newthumbback.SetTexture(newtexture);
                    
                    var newthumb = new HUDQuad('', 40, yspace * locali, 60, 60, g_hudViewInfo, g_hudRoot, 1, true);
                    newthumb.SetTexture(newtexture);
                    
                    g_GUIarray[g_GUIarray.length] = newthumb;
                    g_ThumbArray[g_ThumbArray.length] = newthumb;
                    newthumb.index = locali;
                    newthumb.key = key;
                    var newthumbborder = new HUDQuad('Images/Icons/Thumbborder.png', 40, yspace * locali, 74, 74, g_hudViewInfo, g_hudRoot, 1, true);
                    newthumbborder.SetPosition(40, yspace * locali, -1, 0);
                    
                 //   g_GUIarray[g_GUIarray.length] = newthumbborder;
                 //   g_ThumbArray[g_ThumbArray.length] = newthumbborder;
                //    newthumbborder.index = locali;
                //    newthumbborder.offsetx = 0;
              //      newthumbborder.offsety = 0;
               //     newthumbborder.mouseOut = function () {
              //          this.SetPosition(40 + this.offsetx, yspace * this.index + this.offsety, 0, 1);
              //      };
              //      newthumbborder.mouseOver = function () {
              //          this.SetPosition(40 + this.offsetx, yspace * this.index + this.offsety, 0, 1);
              //      };
                    i++;

                    newthumb.border = newthumbborder;
                    newthumb.newthumbback = newthumbback;
                    newthumb.offsetx = 0;
                    newthumb.offsety = 0;
                    newthumb.miny = count * yspace;
                    newthumb.updateBorder = function () {

                        


                        this.SetPosition(40 + this.offsetx, yspace * this.index + this.offsety, 0, 1);
                        var dist = Math.abs(this.y - g_client.height / 2);
                        dist /= g_client.height / 2.75;
                        dist = 1 - dist;
                        dist += .25;
                        dist = Math.max(0, Math.min(1, dist));

                        this.border.SetPosition(this.x, this.y, this.z - 1, 1);
                        this.newthumbback.SetPosition(this.x, this.y, this.z - .5, 1);

                        this.SetAlpha(dist);
                        this.border.SetAlpha(dist);
                        this.newthumbback.SetAlpha(dist);

                        var scale = 20 * dist + 40;
                        this.SetScale(scale, scale);
                        this.border.SetScale(scale + 4.5 * dist + 9.5, scale + 4.5 * dist + 9.5);
                        this.newthumbback.SetScale(scale, scale);

                    }
                    newthumb.mouseDrag = function (x, y) {
                        var returnval = true;
                        for (var j = 0; j < g_ThumbArray.length; j++) {
                            // g_ThumbArray[j].SetPosition(g_ThumbArray[j].x, g_ThumbArray[j].y + y, g_ThumbArray[j].z, 1);


                            if (y > -2 && y < 2) {
                                if (x < -2 || (x < 0 && g_ThumbArray[j].offsetx == -65)) {
                                    g_ThumbArray[j].offsetx = -65;
                                    returnval = false
                                }
                                else {
                                    g_ThumbArray[j].offsetx = 0;
                                    returnval = true
                                }
                            }
                            if (g_ThumbArray[j].offsetx != -65) {
                                g_ThumbArray[j].offsety += y;
                                this.newthumbback.SetPosition(g_client.width / 2, g_client.height / 2, 0, 1);

                                this.newthumbback.SetScale(g_client.width / 1.5, g_client.width / 1.5);
                                this.newthumbback.SetAlpha(1);
                                bigthumbborder.show();
                            }

                            if (g_ThumbArray[j].offsety > 0 + g_client.height * .5)
                                g_ThumbArray[j].offsety = 0 + g_client.height * .5;

                            if (g_ThumbArray[j].offsety < -this.miny + g_client.height - g_client.height * .5)
                                g_ThumbArray[j].offsety = -this.miny + g_client.height - g_client.height * .5;
                        }

                        return returnval;

                    }
                    newthumb.mouseOver = function () {
                        if (g_dragging) {
                            this.mouseOut();
                            return;
                        }
                        if (this.offsetx == -65)
                            return;
                        drawText(this.key);
                        this.SetAlpha(1);
                        this.SetPosition(g_client.width / 2, g_client.height / 2, 0, 1);

                        this.SetScale(g_client.width / 1.5, g_client.width / 1.5);
                        bigthumbborder.show();
                    };
                    newthumb.mouseOut = function () {
                       
                        this.updateBorder();
                       // this.SetScale(60, 60);
                        bigthumbborder.hide();
                    };
 
                }
            }

            //if there is a problem
        } else {
            alert(exception);
        }
    }

    //load the file from the path
    var loadInfo = o3djs.io.loadArchive(g_pack,path,callback);

    

}

//Load a 3D content file
function loadFile(context, path) {
    if (g_init == true)
        return;
    function callback(pack, parent, exception) {
        g_init = true;
        enableInput(true);
        if (exception) {
            alert("Could not load: " + path + "\n" + exception);
            g_loadingElement.innerHTML = "loading failed.";

        } else {
			//alert("loaded: " + path + "\n" + exception);

            g_loadingElement.innerHTML = "loading finished.";
            // Generate draw elements and setup material draw lists.
            o3djs.pack.preparePack(pack, g_viewInfo);
            bbox = o3djs.util.getBoundingBoxOfTree(g_sceneRoot);
            g_camera.target = g_math.lerpVector(bbox.minExtent, bbox.maxExtent, 0.5);
            
            g_modelCenter = g_camera.target;
            g_camcenter = g_modelCenter;
            var diag = g_math.length(g_math.subVector(bbox.maxExtent,
                                                bbox.minExtent));
            g_camera.eye = g_math.addVector(g_camera.target, [0, 0, 1.5 * diag]);
            g_camera.nearPlane = diag / 1000;
            g_camera.farPlane = diag * 10;

            //find the bounding box max size, and fit the camera to that distance
            var camlength = g_math.length(g_math.subVector(bbox.maxExtent, bbox.minExtent));
            g_modelSize = camlength;
            //keep track of the min bounds of the model
            g_model_min = bbox.minExtent;
            //setup the matrix for the grid, place it on the min y of hte model
            g_grid = new HUDQuad('Images/grid.png', 0, 0, g_math.length(g_math.subVector(bbox.maxExtent, bbox.minExtent)) * 10, g_math.length(g_math.subVector(bbox.maxExtent, bbox.minExtent)) * 10, g_viewInfo, g_sceneRoot, 10,false);
            g_grid.ResetTransforms();
            g_grid.SetPosition(g_modelCenter[0], bbox.minExtent[1] +.00, g_modelCenter[2], 0);
            g_grid.action = function() { };

            g_shadowQuad = new HUDQuad('Images/shadow3.png', 0, 0, bbox.minExtent[2] - bbox.maxExtent[2], bbox.minExtent[0] - bbox.maxExtent[0], g_viewInfo, g_sceneRoot, 1,false);
            g_shadowQuad.ResetTransforms();
            g_shadowQuad.SetPosition(g_modelCenter[0], bbox.minExtent[1] + .01, g_modelCenter[2], 0);
            g_shadowQuad.action = function() { };
            //create the gui widgets, and associate actions
            //place the on the list so they can be hittested

            BuildHUD();
            ShowTextureThumbs(path);
            g_camvec = g_math.normalize(g_camvec);
            //set the default zoom of the camera to 1.2 times the max radius of the model
            g_camvec = g_math.mulVectorScalar(g_camvec, camlength * 1.2);
            //remember that default radius
            g_defaultRadius = camlength * 1.2;
            setClientSize();
            updateCamera();
            updateProjection();

            SetAxis(g_upaxis);
            SetScale(g_sceneRoot, g_unitscale);

            // Manually connect all the materials' lightWorldPos params to the context
            var materials = pack.getObjectsByClassName('o3d.Material');
            for (var m = 0; m < materials.length; ++m) {
                var material = materials[m];

               
                var param = material.getParam('lightWorldPos');
                if (param) {
                    param.bind(g_lightPosParam);
                }
                
                //disable specular
                var spec = material.getParam('specularFactor');
                if (spec) {
                    spec.value = 0;
                }
                
                //disable ambient
                var ambient = material.getParam('ambient');
                if (ambient) {
                    ambient.value = [1,1,1,1];
                }
            }



            g_finished = true;  // for selenium
           
            // Comment out the next line to dump lots of info.
            if (false) {
                o3djs.dump.dump('---dumping context---\n');
                o3djs.dump.dumpParamObject(context);

                o3djs.dump.dump('---dumping root---\n');
                o3djs.dump.dumpTransformTree(g_client.root);

                o3djs.dump.dump('---dumping render root---\n');
                o3djs.dump.dumpRenderNodeTree(g_client.renderGraphRoot);

                o3djs.dump.dump('---dump g_pack shapes---\n');
                var shapes = pack.getObjectsByClassName('o3d.Shape');
                for (var t = 0; t < shapes.length; t++) {
                    o3djs.dump.dumpShape(shapes[t]);
                }

                o3djs.dump.dump('---dump g_pack materials---\n');
                var materials = pack.getObjectsByClassName('o3d.Material');
                for (var t = 0; t < materials.length; t++) {
                    o3djs.dump.dump(
              '  ' + t + ' : ' + materials[t].className +
              ' : "' + materials[t].name + '"\n');
                    o3djs.dump.dumpParams(materials[t], '    ');
                }

                o3djs.dump.dump('---dump g_pack textures---\n');
                var textures = pack.getObjectsByClassName('o3d.Texture');
                for (var t = 0; t < textures.length; t++) {
                    o3djs.dump.dumpTexture(textures[t]);
                }

                o3djs.dump.dump('---dump g_pack effects---\n');
                var effects = pack.getObjectsByClassName('o3d.Effect');
                for (var t = 0; t < effects.length; t++) {
                    o3djs.dump.dump('  ' + t + ' : ' + effects[t].className +
                  ' : "' + effects[t].name + '"\n');
                    o3djs.dump.dumpParams(effects[t], '    ');
                }
            }
        }
    }

    

    // Create a new transform for the loaded file
    var parent = g_ModelPack.createObject('Transform');
    parent.parent = g_client.root;
    if (path != null) {
        g_loadingElement.innerHTML = "Loading: " + path;
        enableInput(false);
        try {
            o3djs.scene.loadScene(g_client, g_ModelPack, parent, path, callback);
        } catch (e) {
            enableInput(true);
            g_loadingElement.innerHTML = "loading failed : " + e;
        }
    }

    //Begin animation!
    Animate();
    return parent;
}

function setClientSize() {
    var newWidth = parseInt(g_client.width);
    var newHeight = parseInt(g_client.height);

    if (newWidth != g_o3dWidth || newHeight != g_o3dHeight) {
        g_o3dWidth = newWidth;
        g_o3dHeight = newHeight;
        
        updateProjection();
       
        // Sets a new area size for arcball.
        g_aball.setAreaSize(g_o3dWidth, g_o3dHeight);
        //if the fullscreen button exists, then move it and the text canvas to the 
        //bottom and side of the screen
        if (g_fullscreenButton != null) {
            g_fullscreenButton.SetPosition(newWidth - 10, 10, 0, 0);
            g_textCanvas.transform.localMatrix = g_math.matrix4.setTranslation(g_textCanvas.transform.localMatrix, [0, g_o3dHeight - 20, 0]);
        }
        g_client.setFullscreenClickRegion(g_o3dWidth - 10, 10, 20, 20, 0);
    }
}

/**
*  Called every frame.
*/
function onRender() {
    // If we don't check the size of the client area every frame we don't get a
    // chance to adjust the perspective matrix fast enough to keep up with the
    // browser resizing us.
    setClientSize();
}

/**
* Creates the client area.
*/
var assetPath;
function init(asset, logo, upaxis, unitscale, failCallback) {

  
    if (g_init == true)
        return;
  
    g_currentupaxis = 'Y';
    if (asset) {
        assetPath = asset;
    }
    if (logo) {
        g_logo = logo;
    }
    if (upaxis) {
        g_upaxis = upaxis;
    }
    if (unitscale) {
        g_unitscale = unitscale;
    }
    o3djs.util.makeClients(initStep2, '', undefined, failCallback);
}

/**
* Initializes O3D and loads the scene into the transform graph.
* @param {Array} clientElements Array of o3d object elements.
*/
var url;
/**
* Initializes O3D and loads the scene into the transform graph.
* @param {Array} clientElements Array of o3d object elements.
*/
function initStep2(clientElements) {

    g_WireFrame = false;
    var path = window.location.href;
    var index = path.lastIndexOf('/');
    var o3dfilename =  path.substring(path.lastIndexOf('='),path.length);
    url = assetPath;//path.substring(0, index + 1) + assetPath;

 
    g_loadingElement = document.getElementById('loading');

    g_o3dElement = clientElements[0];
    g_o3d = g_o3dElement.o3d;
    g_math = o3djs.math;
    g_quaternions = o3djs.quaternions;
    g_client = g_o3dElement.client;
    g_o3dPrimitives = o3djs.primitives;
    g_mainPack = g_client.createPack();
    g_pack = g_client.createPack();
    g_ModelPack = g_client.createPack();
    g_hudRoot = g_pack.createObject('Transform');
    g_sceneRoot = g_pack.createObject('Transform');

    g_sceneRoot.parent = g_client.root;
    g_hudRoot.parent = g_client.root;
    // Create the render graph for a view.
    g_viewInfo = o3djs.rendergraph.createBasicView(
      g_mainPack,
      g_sceneRoot,
      g_client.renderGraphRoot,
	  [1, 1, 1, 1]);    //set the clear color to white

  g_hudViewInfo = o3djs.rendergraph.createBasicView(  
                 g_pack,  
                 g_hudRoot,  
                 g_client.renderGraphRoot);  
   
            // Make sure the hud gets drawn after the 3d stuff  
            g_hudViewInfo.root.priority = g_viewInfo.root.priority + 1;  
   
            // Turn off clearing color for the hud since it would erase the 3d  
            // parts but leave clearing the depth and stencil so the HUD is  
            // unaffected by anything done by the 3d parts.  
            g_hudViewInfo.clearBuffer.clearColorFlag = false;  

            //Set up the 2d orthographic view  
            g_hudViewInfo.drawContext.projection = g_math.matrix4.orthographic(  
               0 + 0.5,  
               g_client.width + 0.5,  
               g_client.height + 0.5,  
               0 + 0.5,  
               0.001,  
               1000);  
   
            g_hudViewInfo.drawContext.view = g_math.matrix4.lookAt(  
               [0, 0, 1],   // eye  
               [0, 0, 0],   // target  
               [0, 1, 0]);  // up   

    g_lastRot = g_math.matrix4.identity();
    g_thisRot = g_math.matrix4.identity();

    var root = g_sceneRoot;

    g_aball = o3djs.arcball.create(100, 100);
    setClientSize();

    // Set the light at the same position as the camera to create a headlight
    // that illuminates the object straight on.
    var paramObject = g_mainPack.createObject('ParamObject');
    g_lightPosParam = paramObject.createParam('lightWorldPos', 'ParamFloat3');
    g_camera.target = [0, 0, 0];
    g_camera.eye = [0, 5, 5];
    //default position and orientation for the camera
    g_camcenter = [0, 0, 0];
    g_camvec = [5, 5, 5];
    updateCamera();

    doload(url)

    o3djs.event.addEventListener(g_o3dElement, 'mouseup', pick);
    o3djs.event.addEventListener(g_o3dElement, 'mousedown', startDragging);
    o3djs.event.addEventListener(g_o3dElement, 'mouseup', stopDragging);
    o3djs.event.addEventListener(g_o3dElement, 'mousemove', drag);
    o3djs.event.addEventListener(g_o3dElement, 'wheel', scrollMe);
    o3djs.event.addEventListener(g_o3dElement, 'keydown', keyDown);
    o3djs.event.addEventListener(g_o3dElement, 'keyup', keyUp);
    
    //create the grid object
    //createGrid();
    
    // Create the global paint object thats used by  draw operations.  
    g_paint = g_pack.createObject('CanvasPaint');

    // Creates an instance of the canvas utilities library.
    g_canvasLib = o3djs.canvas.create(g_pack, g_hudRoot, g_hudViewInfo);

    // Create a canvas that will be used to display the text.
    g_textCanvas = g_canvasLib.createXYQuad(0, (g_client.height-20), 0, 200, 150, true);

    //drawText(o3dfilename);
    
    g_client.setRenderCallback(onRender);
    g_client.setFullscreenClickRegion(g_o3dWidth - 15, 0, 30, 30, 0);
    
}

/**
* Removes any callbacks so they don't get called after the page has unloaded.
*/
function uninit() {
    if (g_init) {
        g_client.cleanup();
        g_client = null;
    }
}

function reset() {
    if (g_init) {

        //reset the reference to the shadowquad
        //should not make a difference, but good idea
        g_shadowQuad = null;
        //This one is very important. If this is not reset, then when we iterate over the 
        //array to generate the mouse events, we are reference null pointers

        //reset this stuff so that the orientation loads property
        sidevec = [1, 0, 0];
        frontvec = [0, 0, 1];
        upvec = [0, 1, 0];
        nextrot = 90;       



        o3djs.event.removeEventListener(g_o3dElement, 'mouseup', pick);
        o3djs.event.removeEventListener(g_o3dElement, 'mousedown', startDragging);
        o3djs.event.removeEventListener(g_o3dElement, 'mouseup', stopDragging);
        o3djs.event.removeEventListener(g_o3dElement, 'mousemove', drag);
        o3djs.event.removeEventListener(g_o3dElement, 'wheel', scrollMe);
        o3djs.event.removeEventListener(g_o3dElement, 'keydown', keyDown);
        o3djs.event.removeEventListener(g_o3dElement, 'keyup', keyUp);
        g_client.clearRenderCallback();
        g_client.cleanup();
        g_init = false;
        g_GUIarray = [];
        g_ThumbArray = [];
    }
}
var assetUrl;
function doload(url) {
    if (url) {
        assetUrl = url;
    }

    try {
        var file = loadFile(g_viewInfo.drawContext, assetUrl);
        file.parent = g_sceneRoot;

      
        
    } catch (ex) {
        alert(ex.message);
    }
}