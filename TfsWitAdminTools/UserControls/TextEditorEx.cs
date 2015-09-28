using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Windows;

namespace TfsWitAdminTools.UserControls
{
    public class TextEditorEx : TextEditor
    {
        #region Syntax

        public static readonly DependencyProperty SyntaxProperty =
            DependencyProperty.Register("Syntax", typeof(string), typeof(TextEditorEx), new FrameworkPropertyMetadata(string.Empty));

        public string Syntax
        {
            get { return (string)GetValue(SyntaxProperty); }
            set
            {
                SetValue(SyntaxProperty, value);
                SetFolding();
            }
        }

        private FoldingManager _foldingManager { get; set; }

        private XmlFoldingStrategy _xmlFoldingStrategy { get; set; }

        public void SetFolding()
        {
            if (Syntax == "XML")
            {
                SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(Syntax);
                _xmlFoldingStrategy = new XmlFoldingStrategy();
                _foldingManager = FoldingManager.Install(TextArea);
                UpdateFolding();
            }
        }

        public void UpdateFolding()
        {
            if (Syntax == "XML")
            {
                _xmlFoldingStrategy.UpdateFoldings(_foldingManager, Document);
            }
        }

        #endregion

        #region DocumentText

        public TextEditorEx()
        {
            base.TextChanged += new EventHandler(TextEditorEx_TextChanged);
        }

        void TextEditorEx_TextChanged(object sender, EventArgs e)
        {
            DocumentText = base.Text;
        }

        public string DocumentText
        {
            get { return (string)GetValue(DocumentTextProperty); }
            set { SetValue(DocumentTextProperty, value); }
        }

        public static readonly DependencyProperty DocumentTextProperty =
            DependencyProperty.Register("DocumentText", typeof(string), typeof(TextEditorEx),
                new PropertyMetadata(new PropertyChangedCallback(OnDocumentTextChanged)));

        private static void OnDocumentTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextEditorEx textEditor = d as TextEditorEx;
            string documentText = e.NewValue as string;
            if (textEditor.Text != documentText)
            {
                textEditor.Text = documentText;

                textEditor.UpdateFolding();
            }
        }

        #endregion

        #region Syntax Name Consts

        public class SyntaxNames
        {
            public static readonly string XML = "XML";
        }

        #endregion
    }
}

