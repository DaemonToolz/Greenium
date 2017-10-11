package main

import (
	"bytes"
	"encoding/json"
	"log"
	"strings"
	"time"

	uuid "github.com/satori/go.uuid"
)

/*
	--------------- CONSTANTS
*/

var ValidMailProvider = [...]string{
	"outlook.com",
	"outlook.fr",
	"hotmail.fr",
	"hotmail.com",
	"gmail.com",
}

/*
	--------------- MODELS
*/

type AccountModel struct {
	ID       string    `json:"ID"`
	Name     string    `json:"Name"`
	FullName string    `json:"FullName"`
	XP       int       `json:"XP"`
	Level    int       `json:"Level"`
	Creation time.Time `json:"Creation"`
	Emails   []string  `json:"Emails"`
}

type AccountCreateRequest struct {
	ID       string   `json:"ID"`
	UID      string   `json:"UID"`
	Name     string   `json:"Name"`
	FullName string   `json:"FullName"`
	Emails   []string `json:"Emails"`
	XpGain   int      `json:"XpGain"`
}

type LoginRequest struct {
	ID string `json:"ID"`
}

type LoginModel struct {
	ID        string `json:"ID"`
	AccountID string `json:"ID"`
}

func Find(id string, channel chan AccountModel) {
	jchannel := make(chan string)
	defer close(jchannel)
	go redisConnector.Get(id, jchannel)
	account := AccountModel{}

	json.Unmarshal([]byte(<-jchannel), &account)
	channel <- account

}

func FindId(id string, channel chan AccountModel) {
	jchannel := make(chan string)
	defer close(jchannel)
	go redisConnector.Get(id, jchannel)
	login := LoginModel{}

	json.Unmarshal([]byte(<-jchannel), &login)

	Find(login.AccountID, channel)
}

func UpdateModel(account AccountModel, channel chan<- bool) {
	go redisConnector.Set(account.ID, account, channel)
}

func AddXP(ID string, xp int, channel chan<- bool) {
	aChannel := make(chan AccountModel)
	defer close(aChannel)
	go Find(ID, aChannel)

	account := <-aChannel

	userXp := (account.XP + xp)
	userLv := account.Level
	LevelXp := (float64(userLv) * (0.25*float64(userLv) + 1.0)) * 500.0

	if float64(userXp) >= LevelXp {
		userXp = userXp - int(LevelXp)
		userLv++
		account.Level = userLv
	}

	account.XP = userXp

	go redisConnector.Set(account.ID, account, channel)
}

func Private_ValidateEmails(emails []string) []string {

	var buffer bytes.Buffer

	valid := make([]string, 0)

	for _, email := range emails {
		contained := false
		for _, provider := range ValidMailProvider {

			buffer.WriteString("@")
			buffer.WriteString(provider)

			contained = strings.Contains(email, buffer.String())

			buffer.Reset()
			if contained {
				break
			}
		}

		if contained {
			valid = append(valid, email)
		}
	}

	return valid
}

func UpdateEmails(ID string, emails []string, channel chan<- bool) {
	aChannel := make(chan AccountModel)

	defer close(aChannel)
	go Find(ID, aChannel)

	valid := Private_ValidateEmails(emails)

	account := <-aChannel
	account.Emails = valid

	go redisConnector.Set(account.ID, account, channel)
}

func Remove(ID string, channel chan<- bool) {
	go redisConnector.Remove(ID, channel)
}

func Create(name string, fullname string, emails []string, myUID string, channel chan AccountModel) {
	bChannel := make(chan bool)
	defer close(bChannel)
	uid := uuid.NewV4().String()

	account := AccountModel{
		Name:     name,
		FullName: fullname,
		ID:       uid,
		XP:       0,
		Level:    0,
		Emails:   Private_ValidateEmails(emails),
		Creation: time.Now(),
	}

	go redisConnector.Set(uid, account, bChannel)
	if <-bChannel == false {
		channel <- AccountModel{ID: "ERR_500INTEX"}
		log.Println("Could not register user")
		return
	}

	logModel := LoginModel{
		AccountID: uid,
		ID:        myUID,
	}

	go redisConnector.Set(uid, logModel, bChannel)
	<-bChannel
	channel <- account
}

func Reset() {
	redisConnector.Flush()
	redisConnector.Save()
}
