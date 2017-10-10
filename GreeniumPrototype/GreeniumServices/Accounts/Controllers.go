package main

import (
	"encoding/json"
	"fmt"
	"time"

	"github.com/go-redis/redis"
)

type RedisClientWrapper struct {
	RedisClient *redis.Client
	Name        string

	Timer    *time.Ticker
	Callback chan struct{}
}

func NewConnection(Address string, Password string, DbIndex int) *RedisClientWrapper {
	//var client *RedisClientWrapper
	client := new(RedisClientWrapper)
	client.RedisClient = redis.NewClient(&redis.Options{
		Addr:     Address,
		Password: Password,
		DB:       DbIndex,
	})

	pong, err := client.RedisClient.Ping().Result()
	fmt.Println(pong, err)
	if err != nil {
		panic(err)
	}

	client.Timer = time.NewTicker(30 * time.Second)
	client.Callback = make(chan struct{})
	go func() {
		for {

			select {
			case <-client.Timer.C:
				client.Save()
			case <-client.Callback:
				client.Save()
				client.Timer.Stop()
				return
			}
		}
	}()

	return client
	// Output: PONG <nil>
}

func CloseConnection(connection *RedisClientWrapper) {
	close(connection.Callback)
	connection.RedisClient.Close()
}

func (client *RedisClientWrapper) Set(key string, value interface{}, channel chan<- bool) {
	jValue, err := json.Marshal(value)
	if err != nil {
		fmt.Println(err)
		return
	}

	result, err := client.RedisClient.SetNX(key, jValue, 0).Result()
	if err != nil {
		fmt.Printf("Error %s occured", err.Error())
		fmt.Println("\n")
		channel <- result
		return
	}
	channel <- result
}

func (client *RedisClientWrapper) Get(key string, channel chan<- string) {

	value, err := client.RedisClient.Get(key).Result()

	if err == redis.Nil {
		value = "{}"
	} else if err != nil {
		panic(err)
	}

	channel <- value
}

func (client *RedisClientWrapper) Remove(key string, channel chan<- bool) {

	result, err := client.RedisClient.Expire(key, time.Second).Result()

	if err != nil {
		channel <- false
		return
	}

	channel <- result
}

func (client *RedisClientWrapper) Save() {
	client.RedisClient.Save()
}

func (client *RedisClientWrapper) Flush() {
	client.RedisClient.FlushDB()
}
