   M S B A M L   `   `   `  ���� L  HGreeniumPrototype, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:GreeniumPrototypeGreeniumPrototype  e7clr-namespace:GreeniumPrototype.Models.CircularProgress)GreeniumPrototype.Models.CircularProgress  T PCefSharp.Wpf, Version=57.0.0.0, Culture=neutral, PublicKeyToken=40c4b6fc221f4138A0clr-namespace:CefSharp.Wpf;assembly=CefSharp.WpfCefSharp.Wpf S OToggleSwitch, Version=1.1.2.0, Culture=neutral, PublicKeyToken=8637099990568f75A0clr-namespace:ToggleSwitch;assembly=ToggleSwitchToggleSwitch "    GreeniumPrototype.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1localclr-namespace:GreeniumPrototype     TcircularProgress7clr-namespace:GreeniumPrototype.Models.CircularProgress     EcefSharp0clr-namespace:CefSharp.Wpf;assembly=CefSharp.Wpf     Cswitch0clr-namespace:ToggleSwitch;assembly=ToggleSwitch       � 
ResizeMode$  NoResize=� � Title$ Greenium��$	��600��$��808.2�� � 	Resources    InfoStyleButton �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value  sbShowRightMenu	 �� TargetProperty
 t� From	 t� To g� DecelerationRatio g� Duration  sbHideRightMenu g� AccelerationRatio  
sbWbReduce W� From	 W� To  sbWbEnlargeD   >GreeniumPrototype.Models.CircularProgress.DoubleToPctConverter  dblToPct  pbarCircularStyle �� 
TargetType �Name J� SnapsToDevicePixels �� 	Converter@   :GreeniumPrototype.Models.CircularProgress.CircularProgress � Value   Value��%�  &	        &	 d     &	 �     &	      &	 E    &	 �    &	 �    7  �� " A�.���  � ��
 ����� P�  �@J�IsMouseOver . . ���  � ��
 �p��� .��t� $	 Margin�$
 
0,0,-150,0X�$ 0,0,0,0X�$ .9Q�$ 0:0:1D��� .��t� $	 Margin�$
 0,0,0,0X�$ 
0,0,-200,0X�$ .9Q�$ 0:0:1D��� .��W� $	 Width�$ 722.0X�$	 522X�$	 0.9Q�$ 0:0:1D��� .��W� $	 Width�$	 522X�$	 722X�$	 0.9Q�$ 0:0:1D�  �� " �.���  � �� �#01D328��  �@�Maximum$	 100Q���  � ��$	 100����  � ��$	 100����  � �� �� " �.R�� $ TemplateRoot�� . .+��� $��Center=�$��Center=�$��32(�
����������� *Value+8     #��z��# z .��� 
�������.+��� -    ��Name$ MainControl��$
��Left=�$	��570��$	��Top=�$	��794��$��
0,0,-0.4,0q����� " ��.R��  � RowDefinitions �� $
��Auto ��� .+��� $
��Auto=�$��Hidden=�.����  ��Name$ HeaderPanel�� K� ZIndex$ 1��-  'System.Windows.Input.KeyboardNavigation   TabIndex$ 1��$��0��$��0��$��2,2,2,0q� K� IsItemsHost . ��  ��Name$ PART_SelectedContentHost��# z #��z��$��SelectedContent��$��1��.@��� $
��75px������ $6��0pack://application:,,,/Assets/Dashboard-icon.png��$
��40px��$
��50px��.��� ���	#FFE5E5E5.+��� -    ��Name$ 
AccountGrp��$��Compte��$
��Left=�$	��205��$	��Top=�$	��370��
�������$��10,0,0,0q�.��� -   $��AccountGrid��$��0,0,191.2,182.4q�.+�  -     Name$ AccountProgress��    Range$	  100Q�$	��137��$��1,17,28.4,-156q�$��Center=����	#FF009FFF �� *XP+��Z� �� -   $��
LevelLabel��$��Center=�$��Center=�$��36(�
�������$��26,61,50.4,-109q�$��90��! �� TextAlignment$! Center=����� *Level+�� $��143,9,-194.6,-33q�$
��Bold"�$��Compte���� $��331,43,-224.6,-66q��� -   $��CurrentXPTB��$��176,38,-84.6,-61q����� *XP+�� $��143,115,-175.6,-138q�$��Prochaine récompense���� $��41,156,75.4,-180q�$
��Bold"�$��Niveau���� $��143,38,-4.6,-61q��� $�XP���� -   " ��Name$" TotalXP��$��256,38,-164.6,-61q��� -   $ AccountGrp_Copy��$��Compte��$
��Left=�$	��310��$��
10,205,0,0q�$	��Top=�$	��370��
��������� -	   $ DashboardGrp��$��Compte-rendu��$
��Left=�$	��515��$��	385,0,0,0q�$	��Top=�$	��399��
�������.��� $��0,15,0.2,337.8q�.+�� $��0,0,220.8,329.6q�.+�  -
   $ AccountCarbonProd��$ 80Q�$	��137��$��1,-1,28.4,-138q�$��Center=��� $��Center=�$��Center=�$��36(�
�������$��10��$��16,43,86.4,-91q�$��64��$! Center=��� $��Center=�$��Center=�$��36(�
�������$��g��$��84,43,60.4,-91q��� $��143,-1,-224.6,-23q�$
��Bold"�$��Production moyenne CO²���� $��165,23,-224.6,-46q��� $
�10g ���� $�sur 10 jours���� $��165,43,-164.6,-66q�$ ��Élément le plus polluant���� $��331,43,-224.6,-66q�� $��0,169,0,-168q�.+�� $��0,0,220.8,329.6q�.+�  -   $ 
CarbonPeak��$ 10Q�$	��137��$��1,-1,28.4,-138q�$��Center=�
���  ���� $��Center=�$��Center=�$��36(�
�������$	��120��$��16,43,86.4,-91q�$��64��$! Center=��� $��Center=�$��Center=�$��36(�
�������$��g��$��84,43,60.4,-91q��� $��143,-1,-224.6,-23q�$
��Bold"�$��2��$��Pic de pollution���� $��165,23,-224.6,-46q�
10/10/2017�� -   $" 
UCID_Param��$
��Left=�$��118,235,0,0q�$
��Wrap=�$��%param%��$	��Top=�$��21���� -   # ��Name$# 	UCID_Name��$��ID Ordinateur��$
��Left=�$��
17,230,0,0q�$	��Top=�$
��Bold"��� -   $# UCID_Usr��$��	ID Unique��$
��Left=�$��
17,256,0,0q�$	��Top=�$
��Bold"��� -   $" UCID_Param2��$
��Left=�$��118,261,0,0q�$
��Wrap=�$��%param%��$	��Top=�$��21���� -   $# UCID_Cnt��$��Online��$
��Left=�$��
17,282,0,0q�$	��Top=�$
��Bold"��� -   $" UCID_Cnt_Param��$
��Left=�$��118,287,0,0q�$
��Wrap=�$��%param%��$	��Top=�$��21��� -   $ OfflineModeGrid��$y�Hidden=�$	��200��$	��100��$��17,313,572.6,106.2q�.+��� -   $# UCID_BXP_Label��$��Saved XP��$��Center=�$
��Left=�$	��Top=�$
��Bold"�$��82��$��21��$��10(��� -   $" UCID_BXP_Param��$��Right=�$
��Wrap=�$��%param%��$	��Top=�$��21���� $
��75px������ $/��)pack://application:,,,/Assets/browser.png��$
��40px��$
��50px��.��� ���	#FFE5E5E5.+�]� $	��Top=�.+�c� $	��Top=�.@��� $#��NavigationCommands.BrowseBack��.���� $��Assets/navigationback.png��$��16��$��16���� $&�� NavigationCommands.BrowseForward��.���� $"��Assets/navigationforward.png��$��16��$��16���� �� -   $��txtUrl��$	��300���� -   $ ��Name$$ GoToBtn��.���� $��Assets/navigationgoto.png��$��16��$��16���� $!��NavigationCommands.GoToPage��.���� $��Assets/refresh.png��$��16��$��16������ " �� P�  �@J�IsMouseOver . . ���  � �� �	#FFB9D7FC�� -   $$ PinBtn��.���� $��Assets/pin.png��$��16��$��16���� -   $$ BookmarkBtn��.���� $��Assets/bookmark.png��$��16��$��16��%  CefSharp.Wpf.ChromiumWebBrowser  -   %  Name$% Browser��$ 99��$
��Left=�$	��495��$��	0,2.8,0,0q�$	��Top=�$	��722��& J� RenderTransformOrigin$& 0.5,0.5&��� -   ' ��Name$' pnlRightMenu��$ 5��( �� Orientation$( 
Horizontal=�$��Right=�$��0,0,-200,0.2q�.+�� 
���"�"�$��	0,0,0,0.2q�$��54��.+��� -   $$ btnRightMenuHide��$��48��$��48��$��0,0,0,479.2q�$y�Hidden=���Z� ���� ) �� ImageSource$) Assets/hide.png���� -   $$ btnRightMenuShow��$��3,2,3,469.4q���Z� ���� $) Assets/show.png���� -   $$ History_btn��$��
4,55,3,418q���Z� ���� $) Assets/history.png���� -   $$ 	btn_Copy1��$��
3,408,4,64q���Z� ���� $) Assets/comment.png���� -   $$ btnMails��$��4,151,3,321q���Z� ���� $) Assets/mails.png���� -    $$ btnBookmarks��$��
4,99,3,373q���Z� ���� $) Assets/bookmark.png���� -!   $$ ContributeBtn��$��4,460,3,10.4q���Z� ���� $) Assets/contribute.png���� ���#AF1719$��1q�$	��200�����	#4C808080$��	0,0,0,0.2q�._� � -"   *  �Name$* SideMenuFrame��+  � NavigationUIVisibility$+ Hidden=�$��Frame��$
��Left=�$	��527��$	��Top=�$	��200��$��-0.8,-0.8,-0.8,-1.4q��� $
��75px������ $/��)pack://application:,,,/Assets/appicon.png��$
��40px��$
��50px��.��� ���	#FFE5E5E5.+��� -#   $ SettingsTabItem��, �� TabStripPlacement$
, Left=�$
��Left=�$	��519��$	��Top=�$	��790��$��
0,0,-0.4,0q�.@��� ��� 
��� d �$	��250��$��50��.+��� $;��5pack://application:,,,/Assets/GeneralSettingsIcon.png��$
��50px��$
��Left=�$��10,0,0,0q��� 
�������$��	Général��$��90��$��Center=�$��20(�.���� $
��Auto=�$��Disabled=�.��� 
�������$	��659��.+�)  #ToggleSwitch.HorizontalToggleSwitch  -$   -  Name$- AutologSwitch��#  ToggleSwitch.ToggleSwitchBase.   CheckedContent$. Autologging��/   UncheckedContent$/ Manual Logging��$��150px��$
��30px��$��7,122,347,507q�  -%   $- 
HostSwitch��$. Enable Hosting��$/ Disable Hosting��$��150px��$
��30px��$��10,19,344,610q�  -&   $- AnonymousSwitch��$. Full Privacy��$/ Partial Privacy��$��150px��$
��30px��$��7,174,347,455q��� -'   $$ HistoryClearBtn��$��Clear History��$
��Left=�$��
10,254,0,0q�$	��Top=�$��96��$��30��$��0q��� -(   $$ CookieClearBtn��$��Clear Cookies��$
��Left=�$��131,254,0,0q�$	��Top=�$��96��$��30��$��0q��� -)   0 ��Name$0 SearchEngineCbox��$��216,319,10,310q�.@��� 1 �� 
IsSelected1 . $��Ecosia���� $��Google���� $��Yahoo���� $��
DuckDuckGo���� $
��Bing���� -*   $# DefaultBrowserLabel��$%��Moteur de recherche par défaut��$
��Left=�$��	7,319,0,0q�$	��Top=�$��30��$	��204��  -+   $- PasswordSaveSwitch��$. Save Ids��$/ No Save��$��150px��$
��30px��$��10,69,344,560q��� -,   $$ PrivacySwitchInfoBtn��$��i��$��177,174,297,455q�$��Italic$�$
��Bold"�$��Script MT Bold)�$��20(�
����������� " ��.R�� .+�A� 
���   �$��0������ " A�2 �� BasedOn#2 [  �� $��Center=�$��Center=��� --   $$ AutologSwitchInfoBtn��$��i��$��177,122,297,507q�$��Italic$�$
��Bold"�$��Script MT Bold)�$��20(�
����������� " ��.R�� .+�A� 
���   �$��0������ " A�#2 [  �� $��Center=�$��Center=��� -.   $$ IdSaveSwitchBtnInfo��$��i��$��177,68,297,560q�$��Italic$�$
��Bold"�$��Script MT Bold)�$��20(�
����������� " ��.R�� .+�A� 
���   �$��0������ " A�#2 [  �� $��Center=�$��Center=��� -/   $$ HostSwitchInfoBtn��$��i��$��177,17,297,610q�$��Italic$�$
��Bold"�$��Script MT Bold)�$��20(�
����������� " ��.R�� .+�A� 
���   �$��0������ " A�#2 [  �� $��Center=�$��Center=��� -0   $" HostSwitchDescriptor��$y�Hidden=�$
��Left=�$��
236,17,0,0q�$
��Wrap=�$-��'Vous permez d'héberger des sites webs ��$	��Top=�$��32��$	��258���� -1   $" PwdSwitchDescriptor��$y�Hidden=�$
��Left=�$��
236,67,0,0q�$
��Wrap=�$5��/Stock par défaut identifiants et mots de passe��$	��Top=�$��32��$	��258���� -2   $" AutologSwitchDescriptor��$y�Hidden=�$
��Left=�$��236,120,0,0q�$
��Wrap=�$<��6Permet de vous connecter à vos emails automatiquement��$	��Top=�$��32��$	��258���� -3   $" PrivacySwitchDescriptor��$y�Hidden=�$
��Left=�$��236,174,0,0q�$
��Wrap=�$B��<Naviguez en limitant la sauvegarde et l'envoi d'informations��$	��Top=�$��32��$	��258���� -4   $# HistoryCountLabel��$��Liens en historique��$
��Left=�$��
10,289,0,0q�$	��Top=�$	��121���� -5   $# HistoryCountParam��$��%param%��$
��Left=�$��136,289,0,0q�$	��Top=�$	��121���� ��� 
��� d �$	��250��$��50��.+��� $:��4pack://application:,,,/Assets/ModuleSettingsIcon.png��$��10,0,0,0q�$
��50px��$
��Left=��� 
�������$��90��$��Modules��$��Center=�$��20(�.��� 
��������� ���� $��New...��$��10(�$
��Bold"�$��40��$��39��$��0q���Z� 