Select R.id,P.Nom_patient,P.Prenom_patient, R.date,R.heure,R.type, R.description, P.Sex
                            from RendezVous R Join Patient P  
                             on R.id_patient=P.id_patient 
                             where date >= GETDATE()
delete from RendezVous where date >= GETDATE();