package main

import "encoding/json"
import "github.com/satori/go.uuid"

type AccountModel struct {
	ID       string `json:"ID"`
	Name     string `json:"Name"`
	FullName string `json:"FullName"`
	XP       int    `json:"XP"`
	Level    int    `json:"Level"`
}

type AccountCreateRequest struct {
	Name     string `json:"Name"`
	FullName string `json:"FullName"`
}

func Find(id string, channel chan AccountModel) {
	jchannel := make(chan string)
	defer close(jchannel)
	go redisConnector.Get(id, jchannel)
	account := AccountModel{}

	json.Unmarshal([]byte(<-jchannel), &account)
	channel <- account

}

func Update(jsonAccount string, channel chan<- bool) {
	account := AccountModel{}
	json.Unmarshal([]byte(jsonAccount), &account)
	go redisConnector.Set(account.ID, account, channel)
}

func Remove(ID string, channel chan<- bool) {
	go redisConnector.Remove(ID, channel)
}

func Create(name string, fullname string, channel chan AccountModel) {
	bChannel := make(chan bool)
	defer close(bChannel)
	uid := uuid.NewV4().String()
	account := AccountModel{
		Name:     name,
		FullName: fullname,
		ID:       uid,
		XP:       0,
		Level:    0,
	}

	go redisConnector.Set(uid, account, bChannel)
	if <-bChannel == false {
		channel <- AccountModel{ID: "ERR_500INTEX"}
		return
	}
	channel <- account
}

func Reset() {
	redisConnector.Flush()
}
