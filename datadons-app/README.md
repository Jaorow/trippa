## to install app run
```bash
cd datadons-app
npm install --legacy-peer-deps
npm run ios
```

## Helpfull stuff for dev
link for icons : [ICONS](https://icons.expo.fyi/Index/FontAwesome/close)

## install packages

``` bash
cd datadons-app
npm install --legacy-peer-deps
```
```--legacy-peer-deps``` is required due to dependency issues

```npm start```
or 
```npx expo run ```


# Fix watchman
- step 1: 
```watchman watch-del-all```
- step 2:
```watchman shutdown-server```


# Windows Fix 
- clean npm cache
```npm cache clean --force```

- (Windows) delete node_modules and package-lock.json
```
rd /s /q "node_modules"
del package-lock.json
```
- update your npm version
```npm install -g npm@latest --force```

- clean npm cache
```npm cache clean --force```

## Using async storage...

// To store data
```await AsyncStorage.setItem('key', 'value');```

// To retrieve data
```const data = await AsyncStorage.getItem('key');```

// To remove data
```await AsyncStorage.removeItem('key');```

## Navigation between pages
```js
import { navigationRef } from './NavigationService';
navigationRef.current?.navigate('Home');
```

