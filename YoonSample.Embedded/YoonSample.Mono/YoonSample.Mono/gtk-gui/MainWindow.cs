
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.Notebook notebook_Main;

	private global::Gtk.VBox vbox_Main;

	private global::Gtk.Frame frame_TcpIPConnect;

	private global::Gtk.Alignment GtkAlignment_TcpIpConnect;

	private global::Gtk.HBox hbox_TcpIpConnect;

	private global::Gtk.Frame frame_TcpClient;

	private global::Gtk.Alignment GtkAlignment_TcpClient;

	private global::Gtk.HBox hbox_TcpClient;

	private global::Gtk.Button button_TcpConnect;

	private global::Gtk.Button button_TcpDisconnect;

	private global::Gtk.Label label_TcpIpClient;

	private global::Gtk.Frame frame_TcpServer;

	private global::Gtk.Alignment GtkAlignment_TcpServer;

	private global::Gtk.HBox hbox_TcpServer;

	private global::Gtk.Button button_TcpListen;

	private global::Gtk.Button button_TcpClose;

	private global::Gtk.Label label_TcpIpServer;

	private global::Gtk.Label label_GroupTcpIp;

	private global::Gtk.Frame frame_SerialConnect;

	private global::Gtk.Alignment GtkAlignment_SerialConnect;

	private global::Gtk.HBox hbox_SerialConnect;

	private global::Gtk.Button button_SerialConnect;

	private global::Gtk.Button button_SerialDisconnect;

	private global::Gtk.Label label_GroupSerial;

	private global::Gtk.Frame frame_Action;

	private global::Gtk.Alignment GtkAlignment_Action;

	private global::Gtk.Table table_Action;

	private global::Gtk.Button button_Action1;

	private global::Gtk.Button button_Action10;

	private global::Gtk.Button button_Action11;

	private global::Gtk.Button button_Action12;

	private global::Gtk.Button button_Action2;

	private global::Gtk.Button button_Action3;

	private global::Gtk.Button button_Action4;

	private global::Gtk.Button button_Action5;

	private global::Gtk.Button button_Action6;

	private global::Gtk.Button button_Action7;

	private global::Gtk.Button button_Action8;

	private global::Gtk.Button button_Action9;

	private global::Gtk.Label label_GroupAction;

	private global::Gtk.Frame frame_Log;

	private global::Gtk.Alignment GtkAlignment_MainLog;

	private global::Gtk.ScrolledWindow GtkScrolledWindow_Log;

	private global::Gtk.TextView textview_Log;

	private global::Gtk.Label label_MainLog;

	private global::Gtk.Label label_TitleMain;

	private global::Gtk.VBox vbox_Setting;

	private global::Gtk.HBox hbox1;

	private global::Gtk.Label label_SettingConnection;

	private global::Gtk.ComboBox combobox_Connection;

	private global::Gtk.Button button_SaveSetting;

	private global::Gtk.Button button_ReloadSetting;

	private global::Gtk.Frame frame1;

	private global::Gtk.Alignment GtkAlignment;

	private global::Gtk.VBox vbox2;

	private global::Gtk.HBox hbox2;

	private global::Gtk.Label label_TcpSettingIP;

	private global::Gtk.Entry entry_TcpSettingIP;

	private global::Gtk.HBox hbox4;

	private global::Gtk.Label label_TcpSettingPort;

	private global::Gtk.Entry entry_TcpSettingPort;

	private global::Gtk.Label label_SettingTcpIp;

	private global::Gtk.Frame frame3;

	private global::Gtk.Alignment GtkAlignment1;

	private global::Gtk.VBox vbox3;

	private global::Gtk.Label label_SettingSerial;

	private global::Gtk.Label label_TitleSetting;

	private global::Gtk.VBox vbox_Action;

	private global::Gtk.HBox hbox_ActionControl;

	private global::Gtk.Button button_SaveAction;

	private global::Gtk.Button button_ReloadAction;

	private global::Gtk.HBox hbox3;

	private global::Gtk.Label label_ActionSelect;

	private global::Gtk.Label label_ActionName;

	private global::Gtk.Label label_ActionValue;

	private global::Gtk.Label label_TitleAction;

	protected virtual void Build()
	{
		global::Stetic.Gui.Initialize(this);
		// Widget MainWindow
		this.WidthRequest = 800;
		this.HeightRequest = 480;
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		this.Resizable = false;
		this.AllowGrow = false;
		this.DefaultWidth = 800;
		this.DefaultHeight = 480;
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.notebook_Main = new global::Gtk.Notebook();
		this.notebook_Main.WidthRequest = 800;
		this.notebook_Main.HeightRequest = 480;
		this.notebook_Main.CanFocus = true;
		this.notebook_Main.Name = "notebook_Main";
		this.notebook_Main.CurrentPage = 1;
		// Container child notebook_Main.Gtk.Notebook+NotebookChild
		this.vbox_Main = new global::Gtk.VBox();
		this.vbox_Main.Name = "vbox_Main";
		this.vbox_Main.Spacing = 6;
		// Container child vbox_Main.Gtk.Box+BoxChild
		this.frame_TcpIPConnect = new global::Gtk.Frame();
		this.frame_TcpIPConnect.Name = "frame_TcpIPConnect";
		this.frame_TcpIPConnect.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame_TcpIPConnect.Gtk.Container+ContainerChild
		this.GtkAlignment_TcpIpConnect = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment_TcpIpConnect.Name = "GtkAlignment_TcpIpConnect";
		this.GtkAlignment_TcpIpConnect.LeftPadding = ((uint)(12));
		// Container child GtkAlignment_TcpIpConnect.Gtk.Container+ContainerChild
		this.hbox_TcpIpConnect = new global::Gtk.HBox();
		this.hbox_TcpIpConnect.Name = "hbox_TcpIpConnect";
		this.hbox_TcpIpConnect.Spacing = 6;
		// Container child hbox_TcpIpConnect.Gtk.Box+BoxChild
		this.frame_TcpClient = new global::Gtk.Frame();
		this.frame_TcpClient.Name = "frame_TcpClient";
		this.frame_TcpClient.BorderWidth = ((uint)(3));
		// Container child frame_TcpClient.Gtk.Container+ContainerChild
		this.GtkAlignment_TcpClient = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment_TcpClient.Name = "GtkAlignment_TcpClient";
		this.GtkAlignment_TcpClient.LeftPadding = ((uint)(12));
		// Container child GtkAlignment_TcpClient.Gtk.Container+ContainerChild
		this.hbox_TcpClient = new global::Gtk.HBox();
		this.hbox_TcpClient.Name = "hbox_TcpClient";
		this.hbox_TcpClient.Spacing = 6;
		this.hbox_TcpClient.BorderWidth = ((uint)(3));
		// Container child hbox_TcpClient.Gtk.Box+BoxChild
		this.button_TcpConnect = new global::Gtk.Button();
		this.button_TcpConnect.WidthRequest = 120;
		this.button_TcpConnect.HeightRequest = 30;
		this.button_TcpConnect.CanFocus = true;
		this.button_TcpConnect.Name = "button_TcpConnect";
		this.button_TcpConnect.UseUnderline = true;
		this.button_TcpConnect.Label = global::Mono.Unix.Catalog.GetString("Connect");
		this.hbox_TcpClient.Add(this.button_TcpConnect);
		global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox_TcpClient[this.button_TcpConnect]));
		w1.Position = 0;
		w1.Expand = false;
		w1.Fill = false;
		// Container child hbox_TcpClient.Gtk.Box+BoxChild
		this.button_TcpDisconnect = new global::Gtk.Button();
		this.button_TcpDisconnect.WidthRequest = 120;
		this.button_TcpDisconnect.HeightRequest = 30;
		this.button_TcpDisconnect.CanFocus = true;
		this.button_TcpDisconnect.Name = "button_TcpDisconnect";
		this.button_TcpDisconnect.UseUnderline = true;
		this.button_TcpDisconnect.Label = global::Mono.Unix.Catalog.GetString("Disconnect");
		this.hbox_TcpClient.Add(this.button_TcpDisconnect);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox_TcpClient[this.button_TcpDisconnect]));
		w2.Position = 1;
		w2.Expand = false;
		w2.Fill = false;
		this.GtkAlignment_TcpClient.Add(this.hbox_TcpClient);
		this.frame_TcpClient.Add(this.GtkAlignment_TcpClient);
		this.label_TcpIpClient = new global::Gtk.Label();
		this.label_TcpIpClient.Name = "label_TcpIpClient";
		this.label_TcpIpClient.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Client</b>");
		this.label_TcpIpClient.UseMarkup = true;
		this.frame_TcpClient.LabelWidget = this.label_TcpIpClient;
		this.hbox_TcpIpConnect.Add(this.frame_TcpClient);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox_TcpIpConnect[this.frame_TcpClient]));
		w5.Position = 0;
		w5.Expand = false;
		w5.Fill = false;
		// Container child hbox_TcpIpConnect.Gtk.Box+BoxChild
		this.frame_TcpServer = new global::Gtk.Frame();
		this.frame_TcpServer.Name = "frame_TcpServer";
		this.frame_TcpServer.BorderWidth = ((uint)(3));
		// Container child frame_TcpServer.Gtk.Container+ContainerChild
		this.GtkAlignment_TcpServer = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment_TcpServer.Name = "GtkAlignment_TcpServer";
		this.GtkAlignment_TcpServer.LeftPadding = ((uint)(12));
		// Container child GtkAlignment_TcpServer.Gtk.Container+ContainerChild
		this.hbox_TcpServer = new global::Gtk.HBox();
		this.hbox_TcpServer.Name = "hbox_TcpServer";
		this.hbox_TcpServer.Spacing = 6;
		this.hbox_TcpServer.BorderWidth = ((uint)(3));
		// Container child hbox_TcpServer.Gtk.Box+BoxChild
		this.button_TcpListen = new global::Gtk.Button();
		this.button_TcpListen.WidthRequest = 120;
		this.button_TcpListen.HeightRequest = 30;
		this.button_TcpListen.CanFocus = true;
		this.button_TcpListen.Name = "button_TcpListen";
		this.button_TcpListen.UseUnderline = true;
		this.button_TcpListen.Label = global::Mono.Unix.Catalog.GetString("Listen");
		this.hbox_TcpServer.Add(this.button_TcpListen);
		global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox_TcpServer[this.button_TcpListen]));
		w6.Position = 0;
		w6.Expand = false;
		w6.Fill = false;
		// Container child hbox_TcpServer.Gtk.Box+BoxChild
		this.button_TcpClose = new global::Gtk.Button();
		this.button_TcpClose.WidthRequest = 120;
		this.button_TcpClose.HeightRequest = 30;
		this.button_TcpClose.CanFocus = true;
		this.button_TcpClose.Name = "button_TcpClose";
		this.button_TcpClose.UseUnderline = true;
		this.button_TcpClose.Label = global::Mono.Unix.Catalog.GetString("Close");
		this.hbox_TcpServer.Add(this.button_TcpClose);
		global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox_TcpServer[this.button_TcpClose]));
		w7.Position = 1;
		w7.Expand = false;
		w7.Fill = false;
		this.GtkAlignment_TcpServer.Add(this.hbox_TcpServer);
		this.frame_TcpServer.Add(this.GtkAlignment_TcpServer);
		this.label_TcpIpServer = new global::Gtk.Label();
		this.label_TcpIpServer.Name = "label_TcpIpServer";
		this.label_TcpIpServer.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Server</b>");
		this.label_TcpIpServer.UseMarkup = true;
		this.frame_TcpServer.LabelWidget = this.label_TcpIpServer;
		this.hbox_TcpIpConnect.Add(this.frame_TcpServer);
		global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox_TcpIpConnect[this.frame_TcpServer]));
		w10.Position = 1;
		w10.Expand = false;
		w10.Fill = false;
		this.GtkAlignment_TcpIpConnect.Add(this.hbox_TcpIpConnect);
		this.frame_TcpIPConnect.Add(this.GtkAlignment_TcpIpConnect);
		this.label_GroupTcpIp = new global::Gtk.Label();
		this.label_GroupTcpIp.Name = "label_GroupTcpIp";
		this.label_GroupTcpIp.LabelProp = global::Mono.Unix.Catalog.GetString("<b>TCP/IP Connect</b>");
		this.label_GroupTcpIp.UseMarkup = true;
		this.frame_TcpIPConnect.LabelWidget = this.label_GroupTcpIp;
		this.vbox_Main.Add(this.frame_TcpIPConnect);
		global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox_Main[this.frame_TcpIPConnect]));
		w13.Position = 0;
		w13.Expand = false;
		w13.Fill = false;
		// Container child vbox_Main.Gtk.Box+BoxChild
		this.frame_SerialConnect = new global::Gtk.Frame();
		this.frame_SerialConnect.Name = "frame_SerialConnect";
		this.frame_SerialConnect.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame_SerialConnect.Gtk.Container+ContainerChild
		this.GtkAlignment_SerialConnect = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment_SerialConnect.Name = "GtkAlignment_SerialConnect";
		this.GtkAlignment_SerialConnect.LeftPadding = ((uint)(12));
		// Container child GtkAlignment_SerialConnect.Gtk.Container+ContainerChild
		this.hbox_SerialConnect = new global::Gtk.HBox();
		this.hbox_SerialConnect.Name = "hbox_SerialConnect";
		this.hbox_SerialConnect.Spacing = 6;
		// Container child hbox_SerialConnect.Gtk.Box+BoxChild
		this.button_SerialConnect = new global::Gtk.Button();
		this.button_SerialConnect.WidthRequest = 150;
		this.button_SerialConnect.HeightRequest = 30;
		this.button_SerialConnect.CanFocus = true;
		this.button_SerialConnect.Name = "button_SerialConnect";
		this.button_SerialConnect.UseUnderline = true;
		this.button_SerialConnect.Label = global::Mono.Unix.Catalog.GetString("Connect");
		this.hbox_SerialConnect.Add(this.button_SerialConnect);
		global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hbox_SerialConnect[this.button_SerialConnect]));
		w14.Position = 0;
		w14.Expand = false;
		w14.Fill = false;
		// Container child hbox_SerialConnect.Gtk.Box+BoxChild
		this.button_SerialDisconnect = new global::Gtk.Button();
		this.button_SerialDisconnect.WidthRequest = 150;
		this.button_SerialDisconnect.HeightRequest = 30;
		this.button_SerialDisconnect.CanFocus = true;
		this.button_SerialDisconnect.Name = "button_SerialDisconnect";
		this.button_SerialDisconnect.UseUnderline = true;
		this.button_SerialDisconnect.Label = global::Mono.Unix.Catalog.GetString("Disconnect");
		this.hbox_SerialConnect.Add(this.button_SerialDisconnect);
		global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.hbox_SerialConnect[this.button_SerialDisconnect]));
		w15.Position = 1;
		w15.Expand = false;
		w15.Fill = false;
		this.GtkAlignment_SerialConnect.Add(this.hbox_SerialConnect);
		this.frame_SerialConnect.Add(this.GtkAlignment_SerialConnect);
		this.label_GroupSerial = new global::Gtk.Label();
		this.label_GroupSerial.Name = "label_GroupSerial";
		this.label_GroupSerial.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Serial Connect</b>");
		this.label_GroupSerial.UseMarkup = true;
		this.frame_SerialConnect.LabelWidget = this.label_GroupSerial;
		this.vbox_Main.Add(this.frame_SerialConnect);
		global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.vbox_Main[this.frame_SerialConnect]));
		w18.Position = 1;
		w18.Expand = false;
		w18.Fill = false;
		// Container child vbox_Main.Gtk.Box+BoxChild
		this.frame_Action = new global::Gtk.Frame();
		this.frame_Action.Name = "frame_Action";
		this.frame_Action.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame_Action.Gtk.Container+ContainerChild
		this.GtkAlignment_Action = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment_Action.Name = "GtkAlignment_Action";
		this.GtkAlignment_Action.LeftPadding = ((uint)(12));
		// Container child GtkAlignment_Action.Gtk.Container+ContainerChild
		this.table_Action = new global::Gtk.Table(((uint)(3)), ((uint)(4)), false);
		this.table_Action.Name = "table_Action";
		this.table_Action.RowSpacing = ((uint)(6));
		this.table_Action.ColumnSpacing = ((uint)(6));
		// Container child table_Action.Gtk.Table+TableChild
		this.button_Action1 = new global::Gtk.Button();
		this.button_Action1.WidthRequest = 120;
		this.button_Action1.HeightRequest = 30;
		this.button_Action1.CanFocus = true;
		this.button_Action1.Name = "button_Action1";
		this.button_Action1.UseUnderline = true;
		this.button_Action1.Label = global::Mono.Unix.Catalog.GetString("Action1");
		this.table_Action.Add(this.button_Action1);
		global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.table_Action[this.button_Action1]));
		w19.XOptions = ((global::Gtk.AttachOptions)(4));
		w19.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table_Action.Gtk.Table+TableChild
		this.button_Action10 = new global::Gtk.Button();
		this.button_Action10.CanFocus = true;
		this.button_Action10.Name = "button_Action10";
		this.button_Action10.UseUnderline = true;
		this.button_Action10.Label = global::Mono.Unix.Catalog.GetString("Action10");
		this.table_Action.Add(this.button_Action10);
		global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.table_Action[this.button_Action10]));
		w20.TopAttach = ((uint)(2));
		w20.BottomAttach = ((uint)(3));
		w20.LeftAttach = ((uint)(1));
		w20.RightAttach = ((uint)(2));
		w20.XOptions = ((global::Gtk.AttachOptions)(4));
		w20.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table_Action.Gtk.Table+TableChild
		this.button_Action11 = new global::Gtk.Button();
		this.button_Action11.CanFocus = true;
		this.button_Action11.Name = "button_Action11";
		this.button_Action11.UseUnderline = true;
		this.button_Action11.Label = global::Mono.Unix.Catalog.GetString("Action11");
		this.table_Action.Add(this.button_Action11);
		global::Gtk.Table.TableChild w21 = ((global::Gtk.Table.TableChild)(this.table_Action[this.button_Action11]));
		w21.TopAttach = ((uint)(2));
		w21.BottomAttach = ((uint)(3));
		w21.LeftAttach = ((uint)(2));
		w21.RightAttach = ((uint)(3));
		w21.XOptions = ((global::Gtk.AttachOptions)(4));
		w21.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table_Action.Gtk.Table+TableChild
		this.button_Action12 = new global::Gtk.Button();
		this.button_Action12.CanFocus = true;
		this.button_Action12.Name = "button_Action12";
		this.button_Action12.UseUnderline = true;
		this.button_Action12.Label = global::Mono.Unix.Catalog.GetString("Action12");
		this.table_Action.Add(this.button_Action12);
		global::Gtk.Table.TableChild w22 = ((global::Gtk.Table.TableChild)(this.table_Action[this.button_Action12]));
		w22.TopAttach = ((uint)(2));
		w22.BottomAttach = ((uint)(3));
		w22.LeftAttach = ((uint)(3));
		w22.RightAttach = ((uint)(4));
		w22.XOptions = ((global::Gtk.AttachOptions)(4));
		w22.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table_Action.Gtk.Table+TableChild
		this.button_Action2 = new global::Gtk.Button();
		this.button_Action2.WidthRequest = 120;
		this.button_Action2.HeightRequest = 30;
		this.button_Action2.CanFocus = true;
		this.button_Action2.Name = "button_Action2";
		this.button_Action2.UseUnderline = true;
		this.button_Action2.Label = global::Mono.Unix.Catalog.GetString("Action2");
		this.table_Action.Add(this.button_Action2);
		global::Gtk.Table.TableChild w23 = ((global::Gtk.Table.TableChild)(this.table_Action[this.button_Action2]));
		w23.LeftAttach = ((uint)(1));
		w23.RightAttach = ((uint)(2));
		w23.XOptions = ((global::Gtk.AttachOptions)(4));
		w23.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table_Action.Gtk.Table+TableChild
		this.button_Action3 = new global::Gtk.Button();
		this.button_Action3.WidthRequest = 120;
		this.button_Action3.HeightRequest = 30;
		this.button_Action3.CanFocus = true;
		this.button_Action3.Name = "button_Action3";
		this.button_Action3.UseUnderline = true;
		this.button_Action3.Label = global::Mono.Unix.Catalog.GetString("Action3");
		this.table_Action.Add(this.button_Action3);
		global::Gtk.Table.TableChild w24 = ((global::Gtk.Table.TableChild)(this.table_Action[this.button_Action3]));
		w24.LeftAttach = ((uint)(2));
		w24.RightAttach = ((uint)(3));
		w24.XOptions = ((global::Gtk.AttachOptions)(4));
		w24.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table_Action.Gtk.Table+TableChild
		this.button_Action4 = new global::Gtk.Button();
		this.button_Action4.WidthRequest = 120;
		this.button_Action4.HeightRequest = 30;
		this.button_Action4.CanFocus = true;
		this.button_Action4.Name = "button_Action4";
		this.button_Action4.UseUnderline = true;
		this.button_Action4.Label = global::Mono.Unix.Catalog.GetString("Action4");
		this.table_Action.Add(this.button_Action4);
		global::Gtk.Table.TableChild w25 = ((global::Gtk.Table.TableChild)(this.table_Action[this.button_Action4]));
		w25.LeftAttach = ((uint)(3));
		w25.RightAttach = ((uint)(4));
		w25.XOptions = ((global::Gtk.AttachOptions)(4));
		w25.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table_Action.Gtk.Table+TableChild
		this.button_Action5 = new global::Gtk.Button();
		this.button_Action5.CanFocus = true;
		this.button_Action5.Name = "button_Action5";
		this.button_Action5.UseUnderline = true;
		this.button_Action5.Label = global::Mono.Unix.Catalog.GetString("Action5");
		this.table_Action.Add(this.button_Action5);
		global::Gtk.Table.TableChild w26 = ((global::Gtk.Table.TableChild)(this.table_Action[this.button_Action5]));
		w26.TopAttach = ((uint)(1));
		w26.BottomAttach = ((uint)(2));
		w26.XOptions = ((global::Gtk.AttachOptions)(4));
		w26.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table_Action.Gtk.Table+TableChild
		this.button_Action6 = new global::Gtk.Button();
		this.button_Action6.CanFocus = true;
		this.button_Action6.Name = "button_Action6";
		this.button_Action6.UseUnderline = true;
		this.button_Action6.Label = global::Mono.Unix.Catalog.GetString("Action6");
		this.table_Action.Add(this.button_Action6);
		global::Gtk.Table.TableChild w27 = ((global::Gtk.Table.TableChild)(this.table_Action[this.button_Action6]));
		w27.TopAttach = ((uint)(1));
		w27.BottomAttach = ((uint)(2));
		w27.LeftAttach = ((uint)(1));
		w27.RightAttach = ((uint)(2));
		w27.XOptions = ((global::Gtk.AttachOptions)(4));
		w27.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table_Action.Gtk.Table+TableChild
		this.button_Action7 = new global::Gtk.Button();
		this.button_Action7.CanFocus = true;
		this.button_Action7.Name = "button_Action7";
		this.button_Action7.UseUnderline = true;
		this.button_Action7.Label = global::Mono.Unix.Catalog.GetString("Action7");
		this.table_Action.Add(this.button_Action7);
		global::Gtk.Table.TableChild w28 = ((global::Gtk.Table.TableChild)(this.table_Action[this.button_Action7]));
		w28.TopAttach = ((uint)(1));
		w28.BottomAttach = ((uint)(2));
		w28.LeftAttach = ((uint)(2));
		w28.RightAttach = ((uint)(3));
		w28.XOptions = ((global::Gtk.AttachOptions)(4));
		w28.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table_Action.Gtk.Table+TableChild
		this.button_Action8 = new global::Gtk.Button();
		this.button_Action8.CanFocus = true;
		this.button_Action8.Name = "button_Action8";
		this.button_Action8.UseUnderline = true;
		this.button_Action8.Label = global::Mono.Unix.Catalog.GetString("Action8");
		this.table_Action.Add(this.button_Action8);
		global::Gtk.Table.TableChild w29 = ((global::Gtk.Table.TableChild)(this.table_Action[this.button_Action8]));
		w29.TopAttach = ((uint)(1));
		w29.BottomAttach = ((uint)(2));
		w29.LeftAttach = ((uint)(3));
		w29.RightAttach = ((uint)(4));
		w29.XOptions = ((global::Gtk.AttachOptions)(4));
		w29.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table_Action.Gtk.Table+TableChild
		this.button_Action9 = new global::Gtk.Button();
		this.button_Action9.CanFocus = true;
		this.button_Action9.Name = "button_Action9";
		this.button_Action9.UseUnderline = true;
		this.button_Action9.Label = global::Mono.Unix.Catalog.GetString("Action9");
		this.table_Action.Add(this.button_Action9);
		global::Gtk.Table.TableChild w30 = ((global::Gtk.Table.TableChild)(this.table_Action[this.button_Action9]));
		w30.TopAttach = ((uint)(2));
		w30.BottomAttach = ((uint)(3));
		w30.XOptions = ((global::Gtk.AttachOptions)(4));
		w30.YOptions = ((global::Gtk.AttachOptions)(4));
		this.GtkAlignment_Action.Add(this.table_Action);
		this.frame_Action.Add(this.GtkAlignment_Action);
		this.label_GroupAction = new global::Gtk.Label();
		this.label_GroupAction.Name = "label_GroupAction";
		this.label_GroupAction.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Action</b>");
		this.label_GroupAction.UseMarkup = true;
		this.frame_Action.LabelWidget = this.label_GroupAction;
		this.vbox_Main.Add(this.frame_Action);
		global::Gtk.Box.BoxChild w33 = ((global::Gtk.Box.BoxChild)(this.vbox_Main[this.frame_Action]));
		w33.Position = 2;
		w33.Expand = false;
		w33.Fill = false;
		// Container child vbox_Main.Gtk.Box+BoxChild
		this.frame_Log = new global::Gtk.Frame();
		this.frame_Log.Name = "frame_Log";
		this.frame_Log.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame_Log.Gtk.Container+ContainerChild
		this.GtkAlignment_MainLog = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment_MainLog.Name = "GtkAlignment_MainLog";
		this.GtkAlignment_MainLog.LeftPadding = ((uint)(12));
		// Container child GtkAlignment_MainLog.Gtk.Container+ContainerChild
		this.GtkScrolledWindow_Log = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow_Log.Name = "GtkScrolledWindow_Log";
		this.GtkScrolledWindow_Log.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow_Log.Gtk.Container+ContainerChild
		this.textview_Log = new global::Gtk.TextView();
		this.textview_Log.CanFocus = true;
		this.textview_Log.Name = "textview_Log";
		this.GtkScrolledWindow_Log.Add(this.textview_Log);
		this.GtkAlignment_MainLog.Add(this.GtkScrolledWindow_Log);
		this.frame_Log.Add(this.GtkAlignment_MainLog);
		this.label_MainLog = new global::Gtk.Label();
		this.label_MainLog.Name = "label_MainLog";
		this.label_MainLog.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Log</b>");
		this.label_MainLog.UseMarkup = true;
		this.frame_Log.LabelWidget = this.label_MainLog;
		this.vbox_Main.Add(this.frame_Log);
		global::Gtk.Box.BoxChild w37 = ((global::Gtk.Box.BoxChild)(this.vbox_Main[this.frame_Log]));
		w37.Position = 3;
		this.notebook_Main.Add(this.vbox_Main);
		// Notebook tab
		this.label_TitleMain = new global::Gtk.Label();
		this.label_TitleMain.WidthRequest = 150;
		this.label_TitleMain.HeightRequest = 30;
		this.label_TitleMain.Name = "label_TitleMain";
		this.label_TitleMain.LabelProp = global::Mono.Unix.Catalog.GetString("Main");
		this.notebook_Main.SetTabLabel(this.vbox_Main, this.label_TitleMain);
		this.label_TitleMain.ShowAll();
		// Container child notebook_Main.Gtk.Notebook+NotebookChild
		this.vbox_Setting = new global::Gtk.VBox();
		this.vbox_Setting.Name = "vbox_Setting";
		this.vbox_Setting.Spacing = 6;
		// Container child vbox_Setting.Gtk.Box+BoxChild
		this.hbox1 = new global::Gtk.HBox();
		this.hbox1.Name = "hbox1";
		this.hbox1.Spacing = 6;
		// Container child hbox1.Gtk.Box+BoxChild
		this.label_SettingConnection = new global::Gtk.Label();
		this.label_SettingConnection.HeightRequest = 25;
		this.label_SettingConnection.Name = "label_SettingConnection";
		this.label_SettingConnection.LabelProp = global::Mono.Unix.Catalog.GetString("Connection :");
		this.hbox1.Add(this.label_SettingConnection);
		global::Gtk.Box.BoxChild w39 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.label_SettingConnection]));
		w39.Position = 0;
		w39.Expand = false;
		w39.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.combobox_Connection = global::Gtk.ComboBox.NewText();
		this.combobox_Connection.WidthRequest = 150;
		this.combobox_Connection.HeightRequest = 25;
		this.combobox_Connection.Name = "combobox_Connection";
		this.hbox1.Add(this.combobox_Connection);
		global::Gtk.Box.BoxChild w40 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.combobox_Connection]));
		w40.Position = 1;
		w40.Expand = false;
		w40.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.button_SaveSetting = new global::Gtk.Button();
		this.button_SaveSetting.WidthRequest = 100;
		this.button_SaveSetting.HeightRequest = 30;
		this.button_SaveSetting.CanFocus = true;
		this.button_SaveSetting.Name = "button_SaveSetting";
		this.button_SaveSetting.UseUnderline = true;
		this.button_SaveSetting.Label = global::Mono.Unix.Catalog.GetString("Save");
		this.hbox1.Add(this.button_SaveSetting);
		global::Gtk.Box.BoxChild w41 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.button_SaveSetting]));
		w41.Position = 3;
		w41.Expand = false;
		w41.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.button_ReloadSetting = new global::Gtk.Button();
		this.button_ReloadSetting.WidthRequest = 100;
		this.button_ReloadSetting.HeightRequest = 30;
		this.button_ReloadSetting.CanFocus = true;
		this.button_ReloadSetting.Name = "button_ReloadSetting";
		this.button_ReloadSetting.UseUnderline = true;
		this.button_ReloadSetting.Label = global::Mono.Unix.Catalog.GetString("Reload");
		this.hbox1.Add(this.button_ReloadSetting);
		global::Gtk.Box.BoxChild w42 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.button_ReloadSetting]));
		w42.Position = 4;
		w42.Expand = false;
		w42.Fill = false;
		this.vbox_Setting.Add(this.hbox1);
		global::Gtk.Box.BoxChild w43 = ((global::Gtk.Box.BoxChild)(this.vbox_Setting[this.hbox1]));
		w43.Position = 0;
		w43.Expand = false;
		w43.Fill = false;
		// Container child vbox_Setting.Gtk.Box+BoxChild
		this.frame1 = new global::Gtk.Frame();
		this.frame1.Name = "frame1";
		this.frame1.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame1.Gtk.Container+ContainerChild
		this.GtkAlignment = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment.Name = "GtkAlignment";
		this.GtkAlignment.LeftPadding = ((uint)(12));
		// Container child GtkAlignment.Gtk.Container+ContainerChild
		this.vbox2 = new global::Gtk.VBox();
		this.vbox2.Name = "vbox2";
		this.vbox2.Spacing = 6;
		// Container child vbox2.Gtk.Box+BoxChild
		this.hbox2 = new global::Gtk.HBox();
		this.hbox2.Name = "hbox2";
		this.hbox2.Spacing = 6;
		// Container child hbox2.Gtk.Box+BoxChild
		this.label_TcpSettingIP = new global::Gtk.Label();
		this.label_TcpSettingIP.WidthRequest = 120;
		this.label_TcpSettingIP.Name = "label_TcpSettingIP";
		this.label_TcpSettingIP.LabelProp = global::Mono.Unix.Catalog.GetString("IP");
		this.label_TcpSettingIP.Wrap = true;
		this.hbox2.Add(this.label_TcpSettingIP);
		global::Gtk.Box.BoxChild w44 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.label_TcpSettingIP]));
		w44.Position = 0;
		w44.Expand = false;
		w44.Fill = false;
		// Container child hbox2.Gtk.Box+BoxChild
		this.entry_TcpSettingIP = new global::Gtk.Entry();
		this.entry_TcpSettingIP.CanFocus = true;
		this.entry_TcpSettingIP.Name = "entry_TcpSettingIP";
		this.entry_TcpSettingIP.IsEditable = true;
		this.entry_TcpSettingIP.InvisibleChar = '●';
		this.hbox2.Add(this.entry_TcpSettingIP);
		global::Gtk.Box.BoxChild w45 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.entry_TcpSettingIP]));
		w45.Position = 1;
		this.vbox2.Add(this.hbox2);
		global::Gtk.Box.BoxChild w46 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox2]));
		w46.Position = 0;
		w46.Expand = false;
		w46.Fill = false;
		// Container child vbox2.Gtk.Box+BoxChild
		this.hbox4 = new global::Gtk.HBox();
		this.hbox4.Name = "hbox4";
		this.hbox4.Spacing = 6;
		// Container child hbox4.Gtk.Box+BoxChild
		this.label_TcpSettingPort = new global::Gtk.Label();
		this.label_TcpSettingPort.WidthRequest = 120;
		this.label_TcpSettingPort.Name = "label_TcpSettingPort";
		this.label_TcpSettingPort.LabelProp = global::Mono.Unix.Catalog.GetString("Port");
		this.label_TcpSettingPort.Wrap = true;
		this.hbox4.Add(this.label_TcpSettingPort);
		global::Gtk.Box.BoxChild w47 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.label_TcpSettingPort]));
		w47.Position = 0;
		w47.Expand = false;
		w47.Fill = false;
		// Container child hbox4.Gtk.Box+BoxChild
		this.entry_TcpSettingPort = new global::Gtk.Entry();
		this.entry_TcpSettingPort.CanFocus = true;
		this.entry_TcpSettingPort.Name = "entry_TcpSettingPort";
		this.entry_TcpSettingPort.IsEditable = true;
		this.entry_TcpSettingPort.InvisibleChar = '●';
		this.hbox4.Add(this.entry_TcpSettingPort);
		global::Gtk.Box.BoxChild w48 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.entry_TcpSettingPort]));
		w48.Position = 1;
		this.vbox2.Add(this.hbox4);
		global::Gtk.Box.BoxChild w49 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox4]));
		w49.Position = 1;
		w49.Expand = false;
		w49.Fill = false;
		this.GtkAlignment.Add(this.vbox2);
		this.frame1.Add(this.GtkAlignment);
		this.label_SettingTcpIp = new global::Gtk.Label();
		this.label_SettingTcpIp.Name = "label_SettingTcpIp";
		this.label_SettingTcpIp.LabelProp = global::Mono.Unix.Catalog.GetString("<b>TCP/IP</b>");
		this.label_SettingTcpIp.UseMarkup = true;
		this.frame1.LabelWidget = this.label_SettingTcpIp;
		this.vbox_Setting.Add(this.frame1);
		global::Gtk.Box.BoxChild w52 = ((global::Gtk.Box.BoxChild)(this.vbox_Setting[this.frame1]));
		w52.Position = 1;
		w52.Expand = false;
		w52.Fill = false;
		// Container child vbox_Setting.Gtk.Box+BoxChild
		this.frame3 = new global::Gtk.Frame();
		this.frame3.Name = "frame3";
		this.frame3.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame3.Gtk.Container+ContainerChild
		this.GtkAlignment1 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment1.Name = "GtkAlignment1";
		this.GtkAlignment1.LeftPadding = ((uint)(12));
		// Container child GtkAlignment1.Gtk.Container+ContainerChild
		this.vbox3 = new global::Gtk.VBox();
		this.vbox3.Name = "vbox3";
		this.vbox3.Spacing = 6;
		this.GtkAlignment1.Add(this.vbox3);
		this.frame3.Add(this.GtkAlignment1);
		this.label_SettingSerial = new global::Gtk.Label();
		this.label_SettingSerial.Name = "label_SettingSerial";
		this.label_SettingSerial.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Serial</b>");
		this.label_SettingSerial.UseMarkup = true;
		this.frame3.LabelWidget = this.label_SettingSerial;
		this.vbox_Setting.Add(this.frame3);
		global::Gtk.Box.BoxChild w55 = ((global::Gtk.Box.BoxChild)(this.vbox_Setting[this.frame3]));
		w55.Position = 2;
		this.notebook_Main.Add(this.vbox_Setting);
		global::Gtk.Notebook.NotebookChild w56 = ((global::Gtk.Notebook.NotebookChild)(this.notebook_Main[this.vbox_Setting]));
		w56.Position = 1;
		// Notebook tab
		this.label_TitleSetting = new global::Gtk.Label();
		this.label_TitleSetting.WidthRequest = 150;
		this.label_TitleSetting.HeightRequest = 30;
		this.label_TitleSetting.Name = "label_TitleSetting";
		this.label_TitleSetting.LabelProp = global::Mono.Unix.Catalog.GetString("Setting");
		this.notebook_Main.SetTabLabel(this.vbox_Setting, this.label_TitleSetting);
		this.label_TitleSetting.ShowAll();
		// Container child notebook_Main.Gtk.Notebook+NotebookChild
		this.vbox_Action = new global::Gtk.VBox();
		this.vbox_Action.Name = "vbox_Action";
		this.vbox_Action.Spacing = 6;
		// Container child vbox_Action.Gtk.Box+BoxChild
		this.hbox_ActionControl = new global::Gtk.HBox();
		this.hbox_ActionControl.Name = "hbox_ActionControl";
		this.hbox_ActionControl.Spacing = 6;
		// Container child hbox_ActionControl.Gtk.Box+BoxChild
		this.button_SaveAction = new global::Gtk.Button();
		this.button_SaveAction.WidthRequest = 100;
		this.button_SaveAction.HeightRequest = 30;
		this.button_SaveAction.CanFocus = true;
		this.button_SaveAction.Name = "button_SaveAction";
		this.button_SaveAction.UseUnderline = true;
		this.button_SaveAction.Label = global::Mono.Unix.Catalog.GetString("Save");
		this.hbox_ActionControl.Add(this.button_SaveAction);
		global::Gtk.Box.BoxChild w57 = ((global::Gtk.Box.BoxChild)(this.hbox_ActionControl[this.button_SaveAction]));
		w57.Position = 1;
		w57.Expand = false;
		w57.Fill = false;
		// Container child hbox_ActionControl.Gtk.Box+BoxChild
		this.button_ReloadAction = new global::Gtk.Button();
		this.button_ReloadAction.WidthRequest = 100;
		this.button_ReloadAction.HeightRequest = 30;
		this.button_ReloadAction.CanFocus = true;
		this.button_ReloadAction.Name = "button_ReloadAction";
		this.button_ReloadAction.UseUnderline = true;
		this.button_ReloadAction.Label = global::Mono.Unix.Catalog.GetString("Reload");
		this.hbox_ActionControl.Add(this.button_ReloadAction);
		global::Gtk.Box.BoxChild w58 = ((global::Gtk.Box.BoxChild)(this.hbox_ActionControl[this.button_ReloadAction]));
		w58.Position = 2;
		w58.Expand = false;
		w58.Fill = false;
		this.vbox_Action.Add(this.hbox_ActionControl);
		global::Gtk.Box.BoxChild w59 = ((global::Gtk.Box.BoxChild)(this.vbox_Action[this.hbox_ActionControl]));
		w59.Position = 0;
		w59.Expand = false;
		w59.Fill = false;
		// Container child vbox_Action.Gtk.Box+BoxChild
		this.hbox3 = new global::Gtk.HBox();
		this.hbox3.Name = "hbox3";
		this.hbox3.Spacing = 6;
		// Container child hbox3.Gtk.Box+BoxChild
		this.label_ActionSelect = new global::Gtk.Label();
		this.label_ActionSelect.HeightRequest = 25;
		this.label_ActionSelect.Name = "label_ActionSelect";
		this.label_ActionSelect.LabelProp = global::Mono.Unix.Catalog.GetString("Sel");
		this.hbox3.Add(this.label_ActionSelect);
		global::Gtk.Box.BoxChild w60 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.label_ActionSelect]));
		w60.Position = 0;
		w60.Expand = false;
		w60.Fill = false;
		// Container child hbox3.Gtk.Box+BoxChild
		this.label_ActionName = new global::Gtk.Label();
		this.label_ActionName.HeightRequest = 25;
		this.label_ActionName.Name = "label_ActionName";
		this.label_ActionName.LabelProp = global::Mono.Unix.Catalog.GetString("Name");
		this.hbox3.Add(this.label_ActionName);
		global::Gtk.Box.BoxChild w61 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.label_ActionName]));
		w61.Position = 1;
		w61.Expand = false;
		w61.Fill = false;
		// Container child hbox3.Gtk.Box+BoxChild
		this.label_ActionValue = new global::Gtk.Label();
		this.label_ActionValue.HeightRequest = 25;
		this.label_ActionValue.Name = "label_ActionValue";
		this.label_ActionValue.LabelProp = global::Mono.Unix.Catalog.GetString("Value");
		this.hbox3.Add(this.label_ActionValue);
		global::Gtk.Box.BoxChild w62 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.label_ActionValue]));
		w62.Position = 2;
		w62.Expand = false;
		w62.Fill = false;
		this.vbox_Action.Add(this.hbox3);
		global::Gtk.Box.BoxChild w63 = ((global::Gtk.Box.BoxChild)(this.vbox_Action[this.hbox3]));
		w63.Position = 1;
		w63.Expand = false;
		w63.Fill = false;
		this.notebook_Main.Add(this.vbox_Action);
		global::Gtk.Notebook.NotebookChild w64 = ((global::Gtk.Notebook.NotebookChild)(this.notebook_Main[this.vbox_Action]));
		w64.Position = 2;
		// Notebook tab
		this.label_TitleAction = new global::Gtk.Label();
		this.label_TitleAction.WidthRequest = 150;
		this.label_TitleAction.HeightRequest = 30;
		this.label_TitleAction.Name = "label_TitleAction";
		this.label_TitleAction.LabelProp = global::Mono.Unix.Catalog.GetString("Action");
		this.notebook_Main.SetTabLabel(this.vbox_Action, this.label_TitleAction);
		this.label_TitleAction.ShowAll();
		this.Add(this.notebook_Main);
		if ((this.Child != null))
		{
			this.Child.ShowAll();
		}
		this.Show();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent);
	}
}
