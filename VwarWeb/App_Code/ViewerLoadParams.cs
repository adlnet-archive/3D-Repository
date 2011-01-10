using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// A container for parameters to be passed to the ViewerLoader javascript class.
/// </summary>
public class ViewerLoadParams
{
    /// <summary>
    /// Determines whether the ViewerLoader can load the data.
    /// </summary>
    public bool IsViewable;

    /// <summary>
    /// The path to the "Public" folder where the Model IHttpHandler is located.
    /// </summary>
    public string BasePath;

    /// <summary>
    /// The filename and parameter template for the ModelIHttpHandler URL.
    /// </summary>
    public string BaseContentUrl;

    /// <summary>
    /// The filename for loading via the Flash viewer.
    /// </summary>
    public string FlashLocation;

    /// <summary>
    /// The filename for loading via the O3D viewer.
    /// </summary>
    public string O3DLocation;

    /// <summary>
    /// The Up Axis to be set for the viewer (Flash and O3D).
    /// </summary>
    public string UpAxis;

    /// <summary>
    /// The Unit Scale (in meters) to be set for the viewer (Flash and O3D).
    /// </summary>
    public string UnitScale;

    /// <summary>
    /// Determines whether to show the screenshot button in the viewer.
    /// </summary>
    public bool ShowScreenshot;


    public ViewerLoadParams()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}