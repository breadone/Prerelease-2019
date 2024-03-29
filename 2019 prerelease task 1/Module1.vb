﻿Module Module1
    Dim recordCount As Integer = LoadValues()

    Sub Main()
        Dim selection As String
        Dim valid As Boolean = False

        'display title sequence
        Console.WriteLine("#########################################################")
        Console.WriteLine("##########  Dwight Schrute's Gym For Muscules  ##########")
        Console.WriteLine("#################  by Pradyun Setti  ####################")
        Console.WriteLine("#########################################################")

        Console.WriteLine("
        1. add new member
        2. search for member
        3. membership ending month")
        Console.WriteLine()

        Console.Write("what action would you like to perform? ")

        Do
            selection = CStr(Console.ReadLine)
            If selection = "1" Or selection = "2" Or selection = "3" Then
                valid = True
                Select Case selection
                    Case 1
                        Call AddNewMember()
                    Case 2
                        Call SearchMembers()
                    Case 3
                        Call EndingMonth()
                End Select
            Else
                Console.Write("enter a valid selection (1, 2, or 3): ")
            End If
        Loop Until valid = True
    End Sub

    Sub AddNewMember()
        Dim id, name, email, joinMonth, activestatus, mon As String
        Dim memberInput As String
        Dim valid As Boolean = False
        joinMonth = CStr(Now).Split("/").Skip(1).First

        Console.WriteLine()
        Console.Write("enter your first and last name: ")
        name = Console.ReadLine

        Do
            Console.Write("enter your email address: ")
            email = Console.ReadLine

            valid = validate(email)
        Loop Until valid = True

        Console.Write("do you want to buy a membership? (y/n): ")
        memberInput = Console.ReadLine

        If memberInput = "y" Then
            activestatus = True
        ElseIf memberInput = "n" Then
            activestatus = False
        Else
            activestatus = True
            Console.Write("unknown input. you're paying for membership.")
        End If

        id = GenerateID(name, email, joinMonth)

        Select Case joinMonth
            Case "01"
                mon = "JAN"
            Case "02"
                mon = "FEB"
            Case "03"
                mon = "MAR"
            Case "04"
                mon = "APR"
            Case "05"
                mon = "MAY"
            Case "06"
                mon = "JUN"
            Case "07"
                mon = "JUL"
            Case "08"
                mon = "AUG"
            Case "09"
                mon = "SEP"
            Case "10"
                mon = "OCT"
            Case "11"
                mon = "NOV"
            Case "12"
                mon = "DEC"
        End Select

        'add to file
        FileOpen(1, "member.txt", OpenMode.Append)
        PrintLine(1, id & "!" & name & "!" & email & "!" & mon & "!" & CStr(activestatus))
        Console.Write("added successfully.")
        FileClose(1)

        'update recordcount
        FileOpen(1, "savedata.txt", OpenMode.Output)
        Print(1, recordCount + 1)
        FileClose(1)

        Call goHome()
    End Sub

    Sub SearchMembers()
        Console.WriteLine()
        Dim names(recordCount), firstNames(recordCount), email(recordCount), values(recordCount), ids(recordCount), search, month(recordCount), active(recordCount) As String
        Dim isfound As Boolean = False
        Dim i As Integer = 0
        Dim count As Integer = 0
        Dim foundvalue(99) As Integer

        'input values from files
        FileOpen(1, "member.txt", OpenMode.Input)
        While Not EOF(1)
            values(i) = LineInput(1)
            i += 1
        End While
        FileClose(1)

        'split values into attributes
        For a As Integer = 0 To recordCount
            ids(a) = values(a).Split("!").First
            names(a) = values(a).Split("!").Skip(1).First
            email(a) = values(a).Split("!").Skip(2).First
            month(a) = values(a).Split("!").Skip(3).First
            active(a) = values(a).Split("!").Skip(4).First
            firstNames(a) = names(a).Split(" ").First.ToLower
        Next

        Console.Write("enter an customer ID or name to search for: ")
        search = Console.ReadLine.ToLower


        For s As Integer = 0 To recordCount
            Select Case search
                Case = ids(s)
                    foundvalue(count) = s
                    isfound = True
                    count += 1
                Case = names(s).ToLower
                    foundvalue(count) = s
                    isfound = True
                    count += 1
                Case = firstNames(s)
                    foundvalue(count) = s
                    isfound = True
                    count += 1
            End Select
        Next s

        'output section
        Console.WriteLine()

        If isfound = True Then
            Console.Write(DisplaySearchTable())
            If count > 1 Then
                For x As Integer = 0 To count - 1
                    Console.Write(ids(foundvalue(x)).PadRight(20) & names(foundvalue(x)).PadRight(20) & email(foundvalue(x)).PadRight(40) & month(foundvalue(x)).PadRight(20) & active(foundvalue(x)).PadRight(20))
                Next x
            ElseIf count = 1 Then
                Console.Write(ids(foundvalue(0)).PadRight(20) & names(foundvalue(0)).PadRight(20) & email(foundvalue(0)).PadRight(40) & month(foundvalue(0)).PadRight(20) & active(foundvalue(0)).PadRight(20))
            End If
        ElseIf isfound = False Then
            Console.Write("your search was not found. make sure the ID number or name is correct")
        End If

        Call goHome()
    End Sub

    Sub EndingMonth()
        Dim values(recordCount), names(recordCount), email(recordCount), ids(recordCount), active(recordCount), month(recordCount), found(99) As String
        Dim i As Integer = 0
        Dim input As String
        Dim flag As Boolean = False


        FileOpen(1, "member.txt", OpenMode.Input)
        While Not EOF(1)
            values(i) = LineInput(1)
            i += 1
        End While
        FileClose(1)

        For a As Integer = 0 To recordCount
            ids(a) = values(a).Split("!").First
            names(a) = values(a).Split("!").Skip(1).First
            email(a) = values(a).Split("!").Skip(2).First
            month(a) = values(a).Split("!").Skip(3).First
            active(a) = values(a).Split("!").Skip(4).First
        Next

        Console.Write("enter the month you would like to view as a number (01 to 12): ")
        input = Console.ReadLine.ToLower

        Console.WriteLine(DisplaySearchTable)
        For j As Integer = 0 To recordCount
            If CInt(values(j).Substring(0, 2)) = input Then
                Console.WriteLine(ids(j).PadRight(20) & names(j).PadRight(20) & email(j).PadRight(40) & month(j).PadRight(20) & active(j).PadRight(20))
                flag = True
            ElseIf j = recordCount And flag = False Then
                Console.WriteLine("no members found")
                'Exit For
            End If
        Next

        Console.Write("these members' membership is expiring this month. do you want to save their information to a new file? (y/n): ")

        If Console.ReadLine = "y" Then
            FileOpen(1, "expiredmembers.txt", OpenMode.Output)
            For m As Integer = 0 To recordCount
                If CInt(values(m).Substring(0, 2)) = input Then
                    PrintLine(1, ids(m).PadRight(20) & names(m).PadRight(20) & email(m).PadRight(40))
                End If
            Next
            Console.WriteLine("success")
            FileClose(1)
        End If


        Call goHome()
    End Sub

    Sub goHome()
        Console.WriteLine()
        Console.Write("enter 'h' to go back home or press the enter key to exit: ")
        If Console.ReadLine.Trim = "h" Then
            Console.Clear()
            Call Main()
        End If
    End Sub

    Function validate(ByRef email As String) As Boolean
        If email.IndexOf("@") >= 0 And email.IndexOf("@") <= email.Length & email.IndexOf(".") >= 0 & email.IndexOf(".") <= email.Length Then
            Return True
        Else
            Return False
        End If
    End Function

    Function GenerateID(ByRef name As String, ByRef email As String, ByRef joinMonth As String) As String
        Dim letter, num(3) As String
        Randomize()

        letter = Asc(name.Substring(0, 1).ToUpper)

        For i As Integer = 0 To 3
            num(i) = CStr(Int(10 * Rnd()))
        Next

        Return joinMonth & letter & num(0) & num(1) & num(2) & num(3)
    End Function


    Function DisplaySearchTable() As String
        Return "ID number".PadRight(20) & "Name".PadRight(20) & "Email".PadRight(40) & "Month joined".PadRight(20) & "member?".PadRight(20)
    End Function

    Function LoadValues()
        Dim values As String
        FileOpen(1, "savedata.txt", OpenMode.Input)
        values = LineInput(1)
        FileClose(1)
        Return values
    End Function

End Module
