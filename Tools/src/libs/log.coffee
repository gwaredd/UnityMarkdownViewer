#------------------------------------------------------------------------------
# https://log4js-node.github.io/log4js-node/index.html

log4js = require "log4js"

logformat = "[%d{yyyy-MM-dd hh:mm:ss}] %[[%p] %m%]"

log4js.configure {
  appenders: {
    console:  { type: 'stdout', layout: { type: 'pattern', pattern: logformat } }
    # file:     { type: 'file', filename: 'app.log' }
  }

  categories: {
    default   : { appenders: [ 'console' ], level: 'debug' }
  }

  levels: {
      trace  : { value: 5000,  colour: 'white' }
      debug  : { value: 10000, colour: 'cyan' }
      info   : { value: 20000, colour: 'white' }
      success: { value: 25000, colour: 'green' }
      warn   : { value: 30000, colour: 'magenta' }
      error  : { value: 40000, colour: 'red' }
      fatal  : { value: 50000, colour: 'red' }
  }
}


# export default logger

module.exports = log4js.getLogger()
