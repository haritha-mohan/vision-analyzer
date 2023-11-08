using ObjCRuntime;
using Vision;

namespace VisionAnalyzer;

public partial class ViewController : NSViewController
{
    private VNImageRequestHandler requestHandler;
    private NSUrl? imageUrl;
    
    protected ViewController(NativeHandle handle) : base (handle)
    {
        // This constructor is required if the view controller is loaded from a xib or a storyboard.
        // Do not put any initialization here, use ViewDidLoad instead.
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        // Do any additional setup after loading the view.
        GenerateJointMenu();
    }
    
    partial void UploadButton(NSObject sender)
    {
        // File picker dialog
        var openPanel = NSOpenPanel.OpenPanel;
        openPanel.CanChooseFiles = true;
        openPanel.CanChooseDirectories = false;
        openPanel.AllowsMultipleSelection = false;

        if (openPanel.RunModal() == 1) // User clicked "Open"
        {
            var fileUrl = openPanel.Urls.First();
            
            ImageFrame.Image = new NSImage(NSData.FromUrl(fileUrl));
            imageUrl = fileUrl;
            FileLabel.StringValue = fileUrl.Path;
        }
    }
    
    partial void OnGetCameraPosition(NSObject sender)
    {
        try 
        {
            if (JointMenu.Title == "Select Joint")
                throw new Exception("Please select a joint first");
            if (imageUrl is null)
                throw new Exception("Please upload an image first");
        
            requestHandler = new VNImageRequestHandler(imageUrl, new NSDictionary());
            
            var request = new VNDetectHumanBodyPose3DRequest();
            requestHandler.Perform (new VNRequest[] { request }, out NSError requestError);
            
            if (requestError is not null)
                throw new Exception(requestError.ToString());
        
            var observation = request.Results.First();
            var enumMember = Enum.Parse(typeof (VNHumanBodyPose3DObservationJointName), JointMenu.Title);
            
            observation.GetCameraRelativePosition(out var vec,
                (VNHumanBodyPose3DObservationJointName)enumMember, out NSError observationError);
            
            if (observationError is not null)
                throw new Exception(requestError.ToString());
            
            PositionVectorLabel.StringValue = vec.ToString();
            
        } catch (Exception e) 
        {
            ShowErrorDialog(e.Message);
        }
    }

    private void GenerateJointMenu()
    {
        NSMenu menu = new();
        foreach (var joint in Enum.GetValues(typeof (VNHumanBodyPose3DObservationJointName)))
        {
            menu.AddItem(new NSMenuItem (joint.ToString()));
        }
        
        JointMenu.Cell.Menu = menu;
        JointMenu.Title = "Select Joint";
        JointMenu.Activated += PopUpButton_SelectedIndexChanged;
    }

    private void PopUpButton_SelectedIndexChanged(object sender, EventArgs e)
    {
        var popUpButton = sender as NSPopUpButton;
        // Get the selected index and selected item
        JointMenu.SelectItem((int) popUpButton.IndexOfSelectedItem);
        JointMenu.Title = popUpButton.TitleOfSelectedItem;
    }

    private static void ShowErrorDialog(string errorMessage)
    {
        var alert = new NSAlert
        {
            AlertStyle = NSAlertStyle.Critical,
            MessageText = "Error",
            InformativeText = errorMessage,
        };

        alert.RunModal();
    }
}
