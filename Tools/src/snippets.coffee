# a reminder

#-- files

fs.existsSync filename

fs.readFile filename, (err,data) ->
  return log.warn err if err?
  console.log data.toString()

data = fs.readFileSync filename
fs.writeFileSync filename, data

glob "some_dir/**/*.log", (err,files) ->
  utils.die err if err?
  _.each files, (f) -> log.info f


#-- reg.exp

str.includes 'searchStr'
str.match /some_regexp/

results = str.match /re_with_(group1)_(group2)_etc/
results[1] = 'group1'
results[2] = 'group2'

