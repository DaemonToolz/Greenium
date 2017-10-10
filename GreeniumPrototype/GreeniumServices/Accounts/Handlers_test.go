package main

import (
	"fmt"
	"testing"
)

var IDs []string

func TestCreateAccount(t *testing.T) {

	tests := []struct {
		Name     string
		FullName string
	}{
		{Name: "Test", FullName: "FullTest"},

		{Name: "Test1", FullName: "FullTest1"},

		{Name: "Test2", FullName: "FullTest2"},

		{Name: "Test3", FullName: "FullTest3"},

		{Name: "Test4", FullName: "FullTest4"},

		{Name: "Test5", FullName: "FullTest5"},

		{Name: "Test6", FullName: "FullTest6"},

		{Name: "Test7", FullName: "FullTest7"},
	}

	IDs = make([]string, 0)

	cChannel := make(chan AccountModel)
	defer close(cChannel)
	//var value AccountModel

	for index, val := range tests {
		fmt.Println("")
		fmt.Printf("Counter %d started", index)
		fmt.Println("")
		go Create(val.Name, val.FullName, cChannel)
		value := <-cChannel

		if value.ID == "ERR_500INTEX" {
			panic(value.ID)
		}

		IDs = append(IDs, value.ID)

		fmt.Printf("%s | %s alias %s, XP:%d and Lv:%d", value.ID, value.FullName, value.Name, value.XP, value.Level)
	}
}

func TestGetAccount(t *testing.T) {

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

		fmt.Printf("%s | %s alias %s, XP:%d and Lv:%d", value.ID, value.FullName, value.Name, value.XP, value.Level)

	}
}

func TestRemoveAccount(t *testing.T) {
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

	Reset()
	CloseConnection(redisConnector)
}
