   M S B A M L   `   `   `  ����
  WpfExp!    WpfExp.DataGridMultiHeader2   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  DataGridMultiHeader��5   	   $	��600��6%   $	��800��62    � 	Resources 5         multiDataGridHeader_gdp �� Orientation  multiDataGridCell_gdp��%w  &	        &	 W    �� 5   
   .R�5      �� $��Stretch=�6   $��Stretch=�5   !   .+�5      �� ._�5      �� $��邮箱��6   $	��340��6)   $��Center=�5   )   5      �� 5      ._�5      �� 
���   �$	��0.5q�5   )   $��Stretch=�5   )   5       �� 5!      ._�5"      �� $	��340��6!   $ 
Horizontal=�6-   .+�5#      �� $��邮箱1��6    $	��170��6.   $��Center=�5$   1   $��Center=�5%   1   �� 5&      
���   �$	��0.5q�5'   1   �� 5(      $��邮箱2��6    $	��170��6.   $��Center=�5)   1   $��Center=�5*   1   5+      5,      5-      5.      �� 5/   
   .R�50      �� $
��Auto��6   $
��Auto��6&   $ 
Horizontal=�64   .+�51      �� ._�52      �� $��1,0,1,0q�6    $	��170��62   $��Center=�54   )   $��Center=�55   )   ��53   )   �� *Email1+55   )   56      �� 57      ._�58      �� 
���   �$	��0.5q�59   )   
���   �$��	0,-1,0,-1q�5;   )   5<      �� 5=      ._�5>      �� $��1,0,1,0q�6    $	��170��62   $��Center=�5@   )   $��Center=�5A   )   ��5?   )   �� *Email2+5A   )   5B      5C      5N      5O      .��5P      � .+�5\   
   �  � RowDefinitions 5]      �� 5^      $��40 �6    �� 5_      .+�5a      � $��0��6    � ColumnDefinitions 5b      �� 5d      ��6'   ��  �� ElementName$ Column1�� �� Path$ ActualWidth��� 5e      ��6'   �� $ Column2��$ ActualWidth��� 5f      ��6'   �� $ Column3��$ ActualWidth��� 5g      ��6'   �� $ Column4��$ ActualWidth�.+�5i      �� $��1��6   $��2��6)   
���   �$��Stretch=�6Q   $��1q�6o   ._�5j      �� $��Center=�6   .��6@   邮箱6D   5k      5l      &   System.Windows.Controls.DataGrid  5m      -   5        $��dataGrid��5n   !      AutoGenerateColumns .  $��1��62      RowHeaderWidth$ 1Q�6?   ��6R   �� *Users+	   Columns	 5o      0  *System.Windows.Controls.DataGridTextColumn  5p      ,  &System.Windows.Controls.DataGridColumn
   Header$
 column 1��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6;   -   5        Column1��1  +System.Windows.Controls.DataGridBoundColumn   Binding 5p   V   �� *Name+  5q      $
 column 2��6)   $ * 6;   -   5        Column2�� 5q   V   �� *Email1+  5r      $
 column 3��6)   $ * 6;   -   5        Column3�� 5r   V   �� *Email2+  5s      $
 column 4��6)   $ * 6;   -   5        Column4�� 5s   V   �� *Age+5u      5v      5w      5x      