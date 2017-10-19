package main

import (
	"log"
	"time"
)

type AddonManager struct {
	Addon []AddonModel

	Timer    *time.Ticker
	Callback chan struct{}
}

func NewManager() *AddonManager {
	//var client *RedisClientWrapper
	client := new(AddonManager)

	client.Timer = time.NewTicker(60 * time.Second)
	client.Callback = make(chan struct{})
	go func() {
		for {

			select {
			case <-client.Timer.C:
				log.Println("Saving Redis database")

			case <-client.Callback:

				client.Timer.Stop()
				return
			}
		}
	}()

	return client
	// Output: PONG <nil>
}

func (client *AddonManager) Exists(addonID string, channel chan<- bool) {

	channel <- false
}

func (client *AddonManager) Get(key string, channel chan<- string) {

	channel <- ""
}

func (client *AddonManager) Discover() {

}
