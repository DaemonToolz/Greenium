package main

import (
	"net/http"
)

type Route struct {
	Name        string
	Method      string
	Pattern     string
	HandlerFunc http.HandlerFunc
}

type Routes []Route

var routes = Routes{
	Route{
		"LoadAccount",
		"GET",
		"/Account/{ID}",
		LoadAccount,
	},
	Route{
		"CreateAccount",
		"POST",
		"/Account/Create",
		CreateAccount,
	},
	Route{
		"GainXP",
		"POST",
		"/Account/GainXP",
		GainXP,
	},
	Route{
		"SetEmails",
		"POST",
		"/Account/SetEmails",
		SetEmails,
	},
	Route{
		"Login",
		"POST",
		"/Account/Login",
		Login,
	},
}
