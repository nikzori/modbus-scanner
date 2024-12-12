# Gidrolock Modbus Scanner

[RU](https://github.com/nikzori/modbus-scanner) | EN

A Modbus scanner with both simple functionality and advanced features. Works for any device connected via Modbus RTU. 
Comes with config files for Gidrolock devices.
Modbus TCP is still WIP.

## Configs and auto-identification

Configs are `.json` files describing basic device info and data that can be polled from it. 
The `checkEntry` object in the config allows to go through every config file in a folder to detect the appropriate config.

## Regular polling

This app can regularly and selectively poll entries from the config file. (Works, but still WIP)

Config fields and possible values:

```js
{
	// Template/device name
	"name" : "Gidrolock Standard Wi-Fi RS-485",

	// Device description
	"description" : "Smart valve controller unit with wired and wireless leak sensor support",

	// the list of entries that can be polled from the device
	// each entry is a separate field that supports polling multiple registers and parsing data into UTF-8, int, etc.
	"entries" : [
		{
			// entry name
			"name": "Modbus ID",

			// register type:
			// "coil", "discrete", "input", "holding"
			"registerType": "holding",

			// starting register address 
			// begins with 0
			"address": 128,

			// number of polled registers
			// default value: 1
			"length": 1,

			// data type for parsing
			// supported data types: bool, uint16, uint32, utf8
			// default value: uint16
			"dataType": "uint16",

			// whether the entry should be polled again
			// if `false`, the entry will be polled only once
			"readOnce": true
		}
	],

	// unique value for a device: model ID, firmware version
	"checkEntry": {
		"registerType": "input", 
		"address": 200,
		"length": 6,
		"dataType": "string",
		
		// value after parsing
		"expectedValue": "SWT485"
	}
}
```


### To-Do
1. Cycle through configs in a folder to select a fitting one
2. Configurable regular polling
3. Modbus TPC support

