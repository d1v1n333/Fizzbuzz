'fizzbuzz
'Monir
'13/05/2026
Module Module1

    Public Class Player
        Public Name As String
        Public Score As Integer
        Private Lives As Integer

        Public Sub New(ByVal playerName As String)
            Name = playerName
            Score = 0
            Lives = 0
        End Sub

        Public Sub UpdateLives(ByVal numLives As Integer)
            Lives = numLives
        End Sub

        Public Function GetLives() As Integer
            Return Lives
        End Function

        Public Sub LoseLife()
            Lives -= 1
        End Sub

        Public Function LivesLeft() As Integer
            Return Lives
        End Function

        Public Function Alive() As Boolean
            Return Lives > 0
        End Function

        Public Sub AddPoint()
            Score = Score + 1
        End Sub

        Public Sub PrintStats()
            Console.WriteLine(Name & " - " & Score & " points")
        End Sub
    End Class

    Public Class Game
        Private Players As List(Of Player)
        Private StartingLives As Integer
        Private CurrentNumber As Integer

        'constructor for when new game starts
        Public Sub New()
            Players = New List(Of Player)
            CurrentNumber = 1
        End Sub

        Public Sub AddPlayer(ByVal p As Player)
            Players.Add(p)
        End Sub

        Public Sub UpdateLives(ByVal numLives As Integer)
            StartingLives = numLives
        End Sub

        Public Sub ExplainRules()
            Console.Clear()
            Console.WriteLine("FIZZBUZZ RULES")
            Console.WriteLine("Players take turns counting upward from 1.")
            Console.WriteLine("Number multiple of 3: say fizz")
            Console.WriteLine("Number multiple of 5: say buzz")
            Console.WriteLine("Number multiple of 3 AND 5: say fizzbuzz")
            Console.WriteLine("Otherwise you say the number as is")
            Console.WriteLine("Wrong answer: lose a life")
            Console.WriteLine("Lose all lives: eliminated")
            Console.WriteLine("Last player standing wins and gains 1 point")
        End Sub

        ' checks correct fizzbuzz answer for current number
        Private Function GetCorrectAnswer(ByVal number As Integer) As String
            If number Mod 3 = 0 And number Mod 5 = 0 Then
                Return "fizzbuzz"
            ElseIf number Mod 3 = 0 Then
                Return "fizz"
            ElseIf number Mod 5 = 0 Then
                Return "buzz"
            Else
                Return number.ToString()
            End If
        End Function

        Public Sub PlayGame()

            ' set starting lives for all players
            For Each p In Players
                p.UpdateLives(StartingLives)
            Next

            Console.Clear()
            Console.WriteLine("Game start")
            Console.WriteLine("Press enter to begin")
            Console.ReadLine()
            Console.Clear()

            Dim GameRunning As Boolean = True
            Dim CurrentPlayerIndex As Integer = 0

            While GameRunning

                Dim CurrentPlayer As Player = Players(CurrentPlayerIndex)

                If CurrentPlayer.Alive() Then

                    ' show current number and player turn
                    Console.WriteLine("Number: " & CurrentNumber)
                    Console.WriteLine(CurrentPlayer.Name & "'s turn")
                    Console.WriteLine("Lives Left: " & CurrentPlayer.GetLives())
                    Console.Write("Answer: ")

                    Dim PlayerInput As String = Console.ReadLine()
                    Dim CorrectAnswer As String = GetCorrectAnswer(CurrentNumber)

                    ' check if answer is correct
                    If PlayerInput.ToLower() = CorrectAnswer Then
                        Console.WriteLine("Correct")
                        CurrentNumber += 1
                    Else
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("Wrong. Correct answer: " & CorrectAnswer)

                        ' lose a life if wrong
                        CurrentPlayer.LoseLife()

                        Console.WriteLine(CurrentPlayer.Name & " has " & CurrentPlayer.GetLives() & " lives remaining")
                        CurrentNumber += 1

                        ' remove player if dead
                        If Not CurrentPlayer.Alive() Then
                            Console.WriteLine(CurrentPlayer.Name & " has been eliminated")
                            Players.RemoveAt(CurrentPlayerIndex)
                            CurrentPlayerIndex -= 1
                        End If

                        Console.ForegroundColor = ConsoleColor.White
                    End If

                    ' check if only one player left (winner)
                    If Players.Count = 1 Then
                        GameRunning = False
                        Dim Winner As Player = Players(0)

                        Winner.AddPoint()

                        Console.ForegroundColor = ConsoleColor.Green
                        Console.WriteLine(Winner.Name & " WINS THE GAME")
                        Console.WriteLine(Winner.Name & " gained 1 point.")
                        Console.ForegroundColor = ConsoleColor.White
                    End If

                    Console.WriteLine()
                    CurrentPlayerIndex += 1

                Else
                    CurrentPlayerIndex += 1
                End If

                ' loop back to first player if needed
                If CurrentPlayerIndex >= Players.Count Then
                    CurrentPlayerIndex = 0
                End If

                ' end game if 1 or 0 players left
                If Players.Count <= 1 Then
                    GameRunning = False
                End If

            End While

            Console.WriteLine("Game over, press Enter to continue...")
            Console.ReadLine()
        End Sub
    End Class

    Dim AllPlayers As New List(Of Player)

    Sub Main()

        Dim ProgramRunning As Boolean = True

        While ProgramRunning
            Console.Clear()
            Console.WriteLine("FIZZBUZZ ")
            Console.WriteLine("=========")
            Console.WriteLine("1. Start New Game")
            Console.WriteLine("2. View Player Scores")
            Console.WriteLine("3. Add New Player")
            Console.WriteLine("4. View Rules")
            Console.WriteLine("5. Example Round")
            Console.WriteLine("6. Exit")
            Console.Write("Choice: ")

            Dim Choice As String = Console.ReadLine()

            Select Case Choice
                Case "1"
                    If AllPlayers.Count < 2 Then
                        Console.WriteLine("Need at least 2 players to start a game")
                        Console.WriteLine("Press Enter to continue...")
                        Console.ReadLine()
                    Else
                        StartNewGame()
                    End If

                Case "2"
                    DisplayAllScores()

                Case "3"
                    AddNewPlayer()

                Case "4"
                    Dim TempGame As New Game()
                    TempGame.ExplainRules()
                    Console.WriteLine("Press Enter to continue...")
                    Console.ReadLine()

                Case "5"
                    ShowExampleRound()

                Case "6"
                    ProgramRunning = False
                    Console.WriteLine("Thanks for playing")
                Case Else
                    Console.WriteLine("Invalid option. Press Enter to continue...")
                    Console.ReadLine()
            End Select
        End While
    End Sub

    Sub StartNewGame()

        Dim NewGame As New Game()

        Console.Clear()
        Console.WriteLine("CREATE NEW GAME")
        Console.WriteLine("Available players: " & AllPlayers.Count)

        Dim PlayersToAdd As New List(Of Player)
        Dim Adding As Boolean = True
        Dim Selection As Integer

        'user chooses what players from the list can be added to the round
        While Adding

            Console.WriteLine("Available players:")

            For i = 0 To AllPlayers.Count - 1
                Console.WriteLine((i + 1) & ". " & AllPlayers(i).Name)
            Next

            Console.Write("Select player number to add (or 0 to finish): ")
            Dim Input As String = Console.ReadLine()

            If Integer.TryParse(Input, Selection) Then

                If Selection = 0 Then
                    If PlayersToAdd.Count >= 2 Then
                        Adding = False
                    Else
                        Console.WriteLine("Need at least 2 players")
                    End If

                ElseIf Selection > 0 And Selection <= AllPlayers.Count Then

                    Dim SelectedPlayer As Player = AllPlayers(Selection - 1)
                    'input validation preventing user from adding same player
                    If PlayersToAdd.Contains(SelectedPlayer) = False Then
                        PlayersToAdd.Add(SelectedPlayer)
                        Console.WriteLine("Added " & SelectedPlayer.Name)
                    Else
                        Console.WriteLine("Already added")
                    End If

                Else
                    Console.WriteLine("Invalid selection")
                End If

            Else
                Console.WriteLine("Invalid input")
            End If
        End While

        For Each p In PlayersToAdd
            NewGame.AddPlayer(p)
        Next

        ' input validation for lives
        Console.Write("Enter number of lives per player (1-10): ")
        Dim Lives As Integer
        Dim LivesInput As String

        Do
            LivesInput = Console.ReadLine()

            If Integer.TryParse(LivesInput, Lives) Then
                If Lives >= 1 And Lives <= 10 Then
                    Exit Do
                End If
            End If

            Console.WriteLine("Enter lives (1-10)")
        Loop

        NewGame.UpdateLives(Lives)
        NewGame.PlayGame()
    End Sub

    Sub AddNewPlayer()

        Console.Clear()
        Console.WriteLine("ADD NEW PLAYER")
        Console.Write("Enter player name: ")

        Dim Name As String = Console.ReadLine()

        For Each p In AllPlayers
            If p.Name.ToLower() = Name.ToLower() Then
                Console.WriteLine("Player already exists")
                Console.WriteLine("Press Enter to continue...")
                Console.ReadLine()
                Return
            End If
        Next

        Dim NewPlayer As New Player(Name)
        AllPlayers.Add(NewPlayer)

        Console.WriteLine("Player '" & Name & "' added successfully.")
        Console.WriteLine("Press Enter to continue...")
        Console.ReadLine()
    End Sub

    Sub DisplayAllScores()

        Console.Clear()
        Console.WriteLine("PLAYER SCORES:")

        If AllPlayers.Count = 0 Then
            Console.WriteLine("No players added yet")
        Else
            For Each p In AllPlayers
                p.PrintStats()
            Next
        End If

        Console.WriteLine("Press Enter to continue...")
        Console.ReadLine()
    End Sub
    Sub ShowExampleRound()

        Console.Clear()


        Console.WriteLine("Players: Dave & Bob")
        Console.WriteLine("Lives: 2 each")
        Console.WriteLine()

        Console.WriteLine("1: 1 (Dave)")
        Console.WriteLine("2: 2 (Bob)")
        Console.WriteLine("3: fizz (Dave)")
        Console.WriteLine("4: 4 (Bob)")
        Console.WriteLine("5: buzz (Dave)")
        Console.WriteLine("6: fizz (Bob)")
        Console.WriteLine("7: 7 (Dave)")
        Console.WriteLine("8: 8 (Bob)")
        Console.WriteLine("9: fizz (Dave) ")
        Console.WriteLine("10: buzz (Bob)")
        Console.WriteLine("11: 11 (Dave)")
        Console.WriteLine("12: fizz (Bob)  wrong, loses life")
        Console.WriteLine("13: 13 (Dave)")
        Console.WriteLine("14: 14 (Bob)")
        Console.WriteLine("15: fizzbuzz (Dave)")
        Console.WriteLine("16: 16 (Bob) wrong, eliminated")

        Console.WriteLine()
        Console.ForegroundColor = ConsoleColor.Red
        Console.WriteLine("Bob has been eliminated")
        Console.ForegroundColor = ConsoleColor.White

        Console.WriteLine()
        Console.ForegroundColor = ConsoleColor.Green
        Console.WriteLine("Dave wins the example round")
        Console.ForegroundColor = ConsoleColor.White

        Console.WriteLine()
        Console.WriteLine("Press Enter to continue...")
        Console.ReadLine()


    End Sub

End Module
