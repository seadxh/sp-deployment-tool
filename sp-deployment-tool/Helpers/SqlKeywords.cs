using System.Text.RegularExpressions;

namespace sp_deployment_tool.Helpers {
    public static class SqlKeywords {

        private static string[] GetKeywords() {
            return new string[] {
                "SELECT", "FROM", "WHERE", "INSERT", "UPDATE", "DELETE",
            "CREATE", "DROP", "ALTER", "TABLE", "VIEW", "INDEX",
            "IF", "BEGIN", "END", "ELSE", "JOIN", "ON", "AND", "OR",
            "NOT", "NULL", "INTO", "VALUES", "SET", "AS", "LIKE", "TOP",
            "ORDER", "BY", "ASC", "DESC", "GROUP", "HAVING", "COUNT",
            "SUM", "MAX", "MIN", "AVG", "INNER", "OUTER", "LEFT", "RIGHT",
            "FULL", "CROSS", "UNION", "ALL", "DISTINCT", "CASE", "WHEN",
            "THEN", "ELSE", "END", "WHILE", "BEGIN", "END", "GOTO",
            "BREAK", "CONTINUE", "RETURN", "TRY", "CATCH", "THROW", "PROCEDURE"
            };
        }

        public static void HighlightSQLSyntax(ref RichTextBox sp_content_input) {
            sp_content_input.SuspendLayout();

            int selectionStart = sp_content_input.SelectionStart;
            int selectionLength = sp_content_input.SelectionLength;

            string text = sp_content_input.Text;

            sp_content_input.SelectAll();
            sp_content_input.SelectionColor = Color.Black;

            var sqlKeywords = GetKeywords();
            foreach (string keyword in sqlKeywords) {
                MatchCollection matches = Regex.Matches(text, $@"\b{keyword}\b", RegexOptions.IgnoreCase);
                foreach (Match match in matches) {
                    sp_content_input.Select(match.Index, match.Length);
                    sp_content_input.SelectionColor = Color.Blue;
                }
            }

            sp_content_input.SelectionStart = selectionStart;
            sp_content_input.SelectionLength = selectionLength;
            sp_content_input.SelectionColor = Color.Black;

            sp_content_input.ResumeLayout();
        }
    }
}
