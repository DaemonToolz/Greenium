package main

import (
	"fmt"
	"testing"
)

var IDs []string

func TestCreateAccount(t *testing.T) {
	redisConnector = NewConnection("localhost:6379", "", 0)

	mails := []string{
		"test@outlook.com",
		"test@gmail.com",
		"test@custom.com",
	}

	tests := []struct {
		Name     string
		FullName string
		Emails   []string
	}{
		{
			Name:     "Test",
			FullName: "FullTest",
			Emails:   mails,
		},

		{Name: "Test1", FullName: "FullTest1", Emails: mails},

		{Name: "Test2", FullName: "FullTest2", Emails: mails},

		{Name: "Test3", FullName: "FullTest3", Emails: mails},

		{Name: "Test4", FullName: "FullTest4", Emails: mails},

		{Name: "Test5", FullName: "FullTest5", Emails: mails},

		{Name: "Test6", FullName: "FullTest6", Emails: mails},

		{Name: "Test7", FullName: "FullTest7", Emails: mails},
	}

	IDs = make([]string, 0)

	cChannel := make(chan AccountModel)
	defer close(cChannel)
	//var value AccountModel

	for index, val := range tests {
		fmt.Println("")
		fmt.Printf("Counter %d started", index)
		fmt.Println("")
		go Create(val.Name, val.FullName, val.Emails, cChannel)
		value := <-cChannel

		if value.ID == "ERR_500INTEX" {
			panic(value.ID)
		}

		IDs = append(IDs, value.ID)

		fmt.Printf("%s | %s alias %s, XP:%d and Lv:%d, ", value.ID, value.FullName, value.Name, value.XP, value.Level, value.Emails)
	}
}

func TestUpdateMail(t *testing.T) {
	redisConnector = NewConnection("localhost:6379", "", 0)

	mails := []string{
		"test1@outlook.com",
		"test5@gmail.com",
		"test@custom.com",
	}

	cChannel := make(chan bool)
	defer close(cChannel)

	aChannel := make(chan AccountModel)
	defer close(aChannel)

	for _, val := range IDs[3:5] {
		go UpdateEmails(val, mails, cChannel)
		go Find(val, aChannel)
		acc := <-aChannel
		fmt.Printf("%s | Mails updated: %s, new mails", val, <-cChannel, acc.Emails)
		fmt.Println("")
	}
}

func TestGetAccount(t *testing.T) {
	redisConnector = NewConnection("localhost:6379", "", 0)

	cChannel := make(chan AccountModel)
	defer close(cChannel)
	//var value AccountModel

	for index, val := range IDs {
		fmt.Println("")
		fmt.Printf("Counter %d started", index)
		fmt.Println("")
		go Find(val, cChannel)
		value := <-cChannel

		if value.ID == "ERR_500INTEX" {
			panic(value.ID)
		}

		fmt.Printf("%s | %s alias %s, XP:%d and Lv:%d, mails ", value.ID, value.FullName, value.Name, value.XP, value.Level, value.Emails)

	}
}

func TestRemoveAccount(t *testing.T) {
	redisConnector = NewConnection("localhost:6379", "", 0)

	if len(IDs) > 0 {
		cChannel := make(chan bool)
		defer close(cChannel)
		//var value AccountModel

		for index, val := range IDs {
			fmt.Println("")
			fmt.Printf("Counter %d started %s", index, val)
			fmt.Println("")
			go Remove(val, cChannel)

			if <-cChannel == false {
				fmt.Printf("Couldn't not remove value %s", val)
			}

		}
	}
	Reset()
	CloseConnection(redisConnector)

}
