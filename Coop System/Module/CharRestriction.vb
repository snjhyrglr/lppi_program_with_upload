Module CharRestriction
    Public charactersAllowed As String = "abcdefghijklmnop .qrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,_-ñÑ"
    Public Sub prohibit(ByVal txt As TextBox)
        Dim theText As String = txt.Text
        Dim Letter As String
        Dim SelectionIndex As Integer = txt.SelectionStart
        Dim Change As Integer

        For x As Integer = 0 To txt.Text.Length - 1
            Letter = txt.Text.Substring(x, 1)
            If charactersAllowed.Contains(Letter) = False Then
                theText = theText.Replace(Letter, String.Empty)
                Change = 1
            End If
        Next

        txt.Text = theText
        txt.Select(SelectionIndex - Change, 0)
    End Sub

    'number
    Public num As String = "1234567890."
    Public Sub number(ByVal txt As TextBox)
        Dim theText As String = txt.Text
        Dim Letter As String
        Dim SelectionIndex As Integer = txt.SelectionStart
        Dim Change As Integer

        For x As Integer = 0 To txt.Text.Length - 1
            Letter = txt.Text.Substring(x, 1)
            If num.Contains(Letter) = False Then
                theText = theText.Replace(Letter, String.Empty)
                Change = 1
            End If
        Next

        txt.Text = theText
        txt.Select(SelectionIndex - Change, 0)
    End Sub

    'email
    Public ema As String = "@_.qwertyuiopasdfghjklzxcvbnmQWERTY123456789UIOPASDFGHJKLZXCVBNM"
    Public Sub email(ByVal txt As TextBox)
        Dim theText As String = txt.Text
        Dim Letter As String
        Dim SelectionIndex As Integer = txt.SelectionStart
        Dim Change As Integer

        For x As Integer = 0 To txt.Text.Length - 1
            Letter = txt.Text.Substring(x, 1)
            If ema.Contains(Letter) = False Then
                theText = theText.Replace(Letter, String.Empty)
                Change = 1
            End If
        Next

        txt.Text = theText
        txt.Select(SelectionIndex - Change, 0)
    End Sub

    'text
    Public t As String = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNMñÑ -.,"
    Public Sub TXT(ByVal txt As TextBox)
        Dim theText As String = txt.Text
        Dim Letter As String
        Dim SelectionIndex As Integer = txt.SelectionStart
        Dim Change As Integer

        For x As Integer = 0 To txt.Text.Length - 1
            Letter = txt.Text.Substring(x, 1)
            If t.Contains(Letter) = False Then
                theText = theText.Replace(Letter, String.Empty)
                Change = 1
            End If
        Next

        txt.Text = theText
        txt.Select(SelectionIndex - Change, 0)
    End Sub

    'contact
    Public c As String = "1234567890+-"
    Public Sub CONT(ByVal txt As TextBox)
        Dim theText As String = txt.Text
        Dim Letter As String
        Dim SelectionIndex As Integer = txt.SelectionStart
        Dim Change As Integer

        For x As Integer = 0 To txt.Text.Length - 1
            Letter = txt.Text.Substring(x, 1)
            If c.Contains(Letter) = False Then
                theText = theText.Replace(Letter, String.Empty)
                Change = 1
            End If
        Next

        txt.Text = theText
        txt.Select(SelectionIndex - Change, 0)
    End Sub

    'password
    Public p As String = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM"
    Public Sub PWORD(ByVal txt As TextBox)
        Dim theText As String = txt.Text
        Dim Letter As String
        Dim SelectionINdex As Integer = txt.SelectionStart
        Dim Change As Integer

        For x As Integer = 0 To txt.Text.Length - 1
            Letter = txt.Text.Substring(x, 1)
            If p.Contains(Letter) = False Then
                theText = theText.Replace(Letter, String.Empty)
                Change = 1
            End If
        Next

        txt.Text = theText
        txt.Select(SelectionINdex - Change, 0)
    End Sub
End Module
