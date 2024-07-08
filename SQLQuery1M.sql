 DECLARE @heure_manuelle time = '23:55:00';
                     SELECT         CASE 
         WHEN DATEPART(hour, heure) = DATEPART(hour, @heure_manuelle)
             AND DATEPART(minute, heure) = DATEPART(minute, @heure_manuelle)
         THEN 'false' 
         ELSE 'true' 
     END AS resultat FROM  RendezVous where  date='2023-06-18' SELECT * FROM RendezVous