Public Class Form1
Private Sub btnIniciarSesion_Click(sender As Object, e As EventArgs) Handles btnIniciarSesion.Click
        ' Credenciales
        Dim usuarioCorrecto As String = "admin"
        Dim passwordCorrecto As String = "1234"

        If txtUsuario.Text = usuarioCorrecto And txtPassword.Text = passwordCorrecto Then
            ' Abrir formulario principal
            Dim f As New PrincipalForm()
            f.Show()

            ' Cerrar login
            Me.Hide()
        Else
            MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
End Class
