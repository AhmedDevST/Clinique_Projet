Select R.id,P.Nom_patient,P.Prenom_patient, R.date,R.heure,R.type, R.description, P.Sex
                             from RendezVous R Join Patient P 
                             on R.id_patient = P.id_patient
                             WHERE CAST(R.date AS DATE) = CAST(GETDATE() AS DATE);
                              select GETDATE();
                              select * from RendezVous
                              select 