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

        private string _syntax;
        public string Syntax
        {
            get { return _syntax; }
            set
            {
                _syntax = value;
                SetFolding();
            }
        }

        private FoldingManager _foldingManager { get; set; }

        private XmlFoldingStrategy _xmlFoldingStrategy { get; set; }

        public void SetFolding()
        {
            if (_syntax == "XML")
            {
                SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(_syntax);
                _xmlFoldingStrategy = new XmlFoldingStrategy();
                _foldingManager = FoldingManager.Install(TextArea);
                UpdateFolding();
            }
        }

        public void UpdateFolding()
        {
            if (_syntax == "XML")
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

        public class SyntaxNames
        {
            public static readonly string XML = "XML";
        }
    }
}

