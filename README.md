# SecretsMan

Easily create JWT signatures without third party dependencies (except SecretsMan, obviously).

Read a key (importantly the private key) from JWK compliant json file:

```csharp

            var data = @"
                {
                  'kty' : 'RSA',
                  'kid' : 'cc34c0a0-bd5a-4a3c-a50d-a2a7db7643df',
                  'use' : 'sig',
                  'n'   : 'pjdss8ZaDfEH6K6U7GeW2nxDqR4IP049fk1fK0lndimbMMVBdPv_hSpm8T8EtBDxrUdi1OHZfMhUixGaut-3nQ4GG9nM249oxhCtxqqNvEXrmQRGqczyLxuh-fKn9Fg--hS9UpazHpfVAFnB5aCfXoNhPuI8oByyFKMKaOVgHNqP5NBEqabiLftZD3W_lsFCPGuzr4Vp0YS7zS2hDYScC2oOMu4rGU1LcMZf39p3153Cq7bS2Xh6Y-vw5pwzFYZdjQxDn8x8BG3fJ6j8TGLXQsbKH1218_HcUJRvMwdpbUQG5nvA2GXVqLqdwp054Lzk9_B_f1lVrmOKuHjTNHq48w',
                  'e'   : 'AQAB',
                  'd'   : 'ksDmucdMJXkFGZxiomNHnroOZxe8AmDLDGO1vhs-POa5PZM7mtUPonxwjVmthmpbZzla-kg55OFfO7YcXhg-Hm2OWTKwm73_rLh3JavaHjvBqsVKuorX3V3RYkSro6HyYIzFJ1Ek7sLxbjDRcDOj4ievSX0oN9l-JZhaDYlPlci5uJsoqro_YrE0PRRWVhtGynd-_aWgQv1YzkfZuMD-hJtDi1Im2humOWxA4eZrFs9eG-whXcOvaSwO4sSGbS99ecQZHM2TcdXeAs1PvjVgQ_dKnZlGN3lTWoWfQP55Z7Tgt8Nf1q4ZAKd-NlMe-7iqCFfsnFwXjSiaOa2CRGZn-Q',
                  'p'   : '4A5nU4ahEww7B65yuzmGeCUUi8ikWzv1C81pSyUKvKzu8CX41hp9J6oRaLGesKImYiuVQK47FhZ--wwfpRwHvSxtNU9qXb8ewo-BvadyO1eVrIk4tNV543QlSe7pQAoJGkxCia5rfznAE3InKF4JvIlchyqs0RQ8wx7lULqwnn0',
                  'q'   : 'ven83GM6SfrmO-TBHbjTk6JhP_3CMsIvmSdo4KrbQNvp4vHO3w1_0zJ3URkmkYGhz2tgPlfd7v1l2I6QkIh4Bumdj6FyFZEBpxjE4MpfdNVcNINvVj87cLyTRmIcaGxmfylY7QErP8GFA-k4UoH_eQmGKGK44TRzYj5hZYGWIC8',
                  'dp'  : 'lmmU_AG5SGxBhJqb8wxfNXDPJjf__i92BgJT2Vp4pskBbr5PGoyV0HbfUQVMnw977RONEurkR6O6gxZUeCclGt4kQlGZ-m0_XSWx13v9t9DIbheAtgVJ2mQyVDvK4m7aRYlEceFh0PsX8vYDS5o1txgPwb3oXkPTtrmbAGMUBpE',
                  'dq'  : 'mxRTU3QDyR2EnCv0Nl0TCF90oliJGAHR9HJmBe__EjuCBbwHfcT8OG3hWOv8vpzokQPRl5cQt3NckzX3fs6xlJN4Ai2Hh2zduKFVQ2p-AF2p6Yfahscjtq-GY9cB85NxLy2IXCC0PF--Sq9LOrTE9QV988SJy_yUrAjcZ5MmECk',
                  'qi'  : 'ldHXIrEmMZVaNwGzDF9WG8sHj2mOZmQpw9yrjLK9hAsmsNr5LTyqWAqJIYZSwPTYWhY4nu2O0EY9G9uYiqewXfCKw_UngrJt8Xwfq1Zruz0YY869zPN4GiE9-9rzdZB33RBw8kIOquY3MK74FMwCihYx_LiU2YTHkaoJ3ncvtvg'
                }
            ".Replace("'", "\"");
            var d = JsonSerializer.Deserialize<JwkData>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var key = JwtSigner.ImportKeyFromJwk(d);
```

then use it to create Jwt tokens:

```csharp

            var somedate = new DateTime(2020, 12, 24);
            var token = JwtSigner.CalculateJwtToken(key, d =>
            {
                d.NotBefore = somedate;
                d.Expires = somedate;
                d.IssuedAt = somedate;
                    
                d.Subject = new ClaimsIdentity("identiteetti");
            });
            Check.That(token).Equals(
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImNjMzRjMGEwLWJkNWEtNGEzYy1hNTBkLWEyYTdkYjc2NDNkZiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2MDc2MTYwOTksImV4cCI6MTYwNzYxOTY5OSwiaWF0IjoxNjA3NjE2MDk5fQ.DovoyF_6l8PDL-AgQDMt2WgCyd82nkr4ry8MwRVhxpD-rvJzUmR9PQXtDntAW77RZCre-9YS5KRBiI2Vf2sN269wuKUjmHzL-cYLvP8S05i5hB_ZpHVqRzuiXQAiCpPUHW76ODA1dT6AIcdgkWdDCvKpBfJXsFh0DTwRAD1hgVtjORRMx4RzfvLfPwsUUSKpfxBOMzSQlnzj0eszuWRS6Dl0KCHREPs12FVXYOC6mM3d-DS4tNSG8EgvU19eUeULpC86uSk7_sdDwgARtJOvqKl87ywbJOqhbNb595PGCxjrMcQOQk4vrqKWEYnObF7s6qFDvGIVguD1S9rmyzXgmQ");

```


